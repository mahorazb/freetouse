﻿using System;
using System.Data;
using GTANetworkAPI;
using Newtonsoft.Json;
using MySqlConnector;
using GolemoSDK;
using System.Collections.Generic;
using Golemo.GUI;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Golemo.Core
{
    class ReportSys : Script
    {
        private class Report
        {
            public int ID { get; set; }
            public string Author { get; set; }
            public int AuthorID { get; set; }
            public string Question { get; set; }
            public string Response { get; set; }
            public string BlockedBy { get; set; }
            
            public DateTime OpenedDate { get; set; }
            public DateTime ClosedDate { get; set; }

            public bool Status { get; set; }

            public void Send(Player someone = null)
            {
                if (someone == null)
                {
                    foreach (Player target in NAPI.Pools.GetAllPlayers())
                    {
                        if (!Main.Players.ContainsKey(target)) continue;
                        if (Main.Players[target].AdminLVL < adminLvL) continue;

                        Trigger.ClientEvent(target, "addreport", ID, Author, AuthorID, Question);
                    }
                }
                else
                {
                    if (!Main.Players.ContainsKey(someone)) return;
                    if (Main.Players[someone].AdminLVL < adminLvL) return;
                    
                    Trigger.ClientEvent(someone, "addreport", ID, Author, AuthorID, Question);
                }
            }
        }
        private static Dictionary<int, Report> Reports;
        private static nLog Log = new nLog("ReportSys");

        private static int adminLvL = 1;

        public static void Init()
        {
            try
            {
                Reports = new Dictionary<int, Report>();

                string cmd = @"TRUNCATE questions;";
                MySQL.Query(cmd);

                //DataTable result = MySQL.QueryRead(cmd);
                //if (result is null) return;
                //foreach(DataRow row in result.Rows)
                //{
                //    if (Convert.ToBoolean((sbyte)row[7]) != false) continue;

                //    Reports.Add((int)row[0], new Report
                //    {
                //        ID = (int)row[0],
                //        Author = row[1].ToString(),
                //        Question = Main.BlockSymbols(row[2].ToString()),
                //        BlockedBy = row[3].ToString(),
                //        Response = Main.BlockSymbols(row[4].ToString()),
                //        OpenedDate = (DateTime)row[5],
                //        ClosedDate = (DateTime)row[6],
                //        Status = Convert.ToBoolean((sbyte)row[7])
                //    });
                //}

            } catch(Exception e)
            {
                Log.Write("Init: " + e.ToString(), nLog.Type.Error);
            }
        }
        public static void onAdminLoad(Player client)
        {
            try
            {
                foreach (Report report in Reports.Values)
                {
                    report.Send(client);
                }

            } catch(Exception e)
            {
                Log.Write("onAdminLoad: " + e.ToString(), nLog.Type.Error);
            }
        }
        
        #region Remote Events
        //Админ взял репорт на себя
        [RemoteEvent("takereport")]
        public void ReportTake(Player client, int id, bool retrn = false)
        {
            if (Main.Players[client].AdminLVL <= 0) return;
            Log.Debug($"Report take: {id} {retrn}");
            if (!Reports.ContainsKey(id))
            {
                Remove(id, client);
                return;
            }
            
            if (Reports[id].Status)
            {
                Remove(id, client);
                return;
            }

            foreach (Player target in NAPI.Pools.GetAllPlayers())
            {
                if (!Main.Players.ContainsKey(target)) continue;
                if (Main.Players[target].AdminLVL < adminLvL) continue;

                if (retrn) Trigger.ClientEvent(target, "setreport", id, "");
                else Trigger.ClientEvent(target, "setreport", id, client.Name);
            }
        }
        [RemoteEvent("sendreport")]
        public void ReportSend(Player player, int ID, string answer)
        {
            if (Main.Players[player].AdminLVL <= 0) return;
            Log.Debug($"Report send: {ID} {answer}");
            if (!Reports.ContainsKey(ID)) return;
            if (!Reports[ID].Status)
            {
                AddAnswer(player, ID, answer);
            }
            else
            {
                player.SendChatMessage("Эта жалоба более недоступна для изменения.");
                Remove(ID, player);
            }
        }
        #endregion
        
        public static void AddReport(Player player, string question)
        {
            try
            {
                question = Main.BlockSymbols(question);
                player.SetData("NEXT_REPORT", DateTime.Now.AddMinutes(2));
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы отправили вопрос: {question}", 3000);
                player.SetData("IS_REPORT", true);

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "INSERT INTO `questions` (`Author`,`Question`,`Opened`,`Closed`) VALUES (@pn,@q,@time,@ntime); SELECT LAST_INSERT_ID();";
                cmd.Parameters.AddWithValue("@pn", player.Name);
                cmd.Parameters.AddWithValue("@q", question);
                cmd.Parameters.AddWithValue("@time", MySQL.ConvertTime(DateTime.Now));
                cmd.Parameters.AddWithValue("@ntime",MySQL.ConvertTime(DateTime.MinValue));

                DataTable dt = MySQL.QueryRead(cmd);

                int id = Convert.ToInt32(dt.Rows[0][0]);
                Report report = new Report
                {
                    ID = id,
                    Author = player.Name,
                    AuthorID = player.Value,
                    Question = question,
                    BlockedBy = "",
                    Response = "",
                    Status = false,
                    OpenedDate = DateTime.Now,
                    ClosedDate = DateTime.MinValue
                };
                report.Send();
                Reports.Add(id, report);
            }
            catch (Exception e)
            {
                Log.Write(e.ToString(), nLog.Type.Error);
            }
        }

        private static void AddAnswer(Player player, int repID, string response)
        {
            try
            {
                response = Main.BlockSymbols(response);

                if (!Main.Players.ContainsKey(player)) return;
                if (Main.Players[player].AdminLVL < adminLvL) return;

                if (!Reports.ContainsKey(repID)) return;

                DateTime now = DateTime.Now;

                try
                {
                    Player target = NAPI.Player.GetPlayerFromName(Reports[repID].Author);
                    if (target is null)
                    {
                        Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Игрок не найден!", 3000);
                    }
                    else
                    {
                        target.SendChatMessage($"~r~Ответ от {player.Name} ({player.Value}): {response}");

                        #region ReportPanel
                        List<object> newdata = new List<object>() { repID, player.Name, response, Reports[repID].Question };
                        string json = JsonConvert.SerializeObject(newdata);
                        Trigger.ClientEvent(target, "reportAnswer", json);
                        #endregion

                        Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Ответ от {player.Name}: {response}", 5000);
                        foreach (var p in Main.Players.Keys.ToList())
                        {
                            if (Main.Players[p].AdminLVL >= adminLvL)
                            {
                                p.SendChatMessage($"~y~[ANSWER] {player.Name}({player.Value})->{target.Name}({target.Value}): {response}");
                            }
                        }
                        GameLog.Admin(player.Name, $"answer({response})", target.Name);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write($"PlayerAnswer:\n" + ex.ToString(), nLog.Type.Error);
                }
                
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "UPDATE questions SET Respondent=@resp,Response=@res,Status=@st,Closed=@time WHERE ID=@repid LIMIT 1";
                cmd.Parameters.AddWithValue("@resp", player.Name);
                cmd.Parameters.AddWithValue("@res", response);
                cmd.Parameters.AddWithValue("@st", true);
                cmd.Parameters.AddWithValue("@time", MySQL.ConvertTime(now));
                cmd.Parameters.AddWithValue("@repid", repID);
                MySQL.Query(cmd);

                Reports[repID].Author = player.Name;
                Reports[repID].Response = response;
                Reports[repID].ClosedDate = now;
                Reports[repID].Status = true;
                
                Remove(repID);
            }
            catch (Exception e)
            {
                Log.Write(e.ToString(), nLog.Type.Error);
            }
        }
        
        private static void Remove(int ID_, Player someone = null)
        {
            try
            {
                Log.Debug($"Remove {ID_}");
                if (someone == null)
                {
                    foreach (Player target in NAPI.Pools.GetAllPlayers())
                    {
                        if (!Main.Players.ContainsKey(target)) continue;
                        if (Main.Players[target].AdminLVL < adminLvL) continue;

                        Trigger.ClientEvent(target, "delreport", ID_);
                    }
                }
                else
                {
                    if (!Main.Players.ContainsKey(someone)) return;
                    if (Main.Players[someone].AdminLVL < adminLvL) return;

                    Trigger.ClientEvent(someone, "delreport", ID_);
                }
                Reports.Remove(ID_);
            }
            catch (Exception e)
            {
                Log.Write(e.ToString(), nLog.Type.Error);
            }
        }
    }
}
