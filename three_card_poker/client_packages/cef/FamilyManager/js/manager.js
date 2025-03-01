var familyManager = new Vue({
  el: '#app',
  data: {
    active: false,
    activeModal: false,
    activeDelete: false,
    activeChangeRank: false,
    activeDisbandModal: false,
    header: null,
    page: 0,
    desc_1: null,
    desc_2: null,
    newdesc_1: null,
    newdesc_2: null,
    allranks: [],
    familyrank: 0,
    familyrankname: null,
    familyimg: "",
    member: null,
    members: [[false, 99, "ADMIN_SERVER", 10, "Rank10"]],
    membermax: 0,
    membercount: 0,
    membersonline: 0,
    changeRank: null,
    modalRank: false,
    reasonKick: null,
	  invitePlayerID: null,
  },
  methods: {
    setinfo(json) {
      this.header = json[6]
      this.familyrank = json[0]
      this.familyrankname = json[1]
      this.familyimg = json[5]
      this.membermax = json[2]
      this.membercount = json[3]
      this.membersonline = json[4]
      this.desc_1 = json[7]
      this.desc_2 = json[8]
      this.allranks = json[9]
    },
    invitePlayer(value) {
      mp.trigger("invitePlayerToFamily", value);
	  this.closeMenu()
    },
    changerank(member) {
      this.member = member
      this.modalRank = true
      this.activeModal = true
    },
    deleteMember(member) {
      this.member = member
      this.activeModal = true
      this.activeDelete = true
    },
    changePage(value) {
      if(value == 1) mp.trigger("loadfamilymemberstomenu", "client")
      this.page = value
    },
    btnAccept() {
      if(this.activeModal)
      {
        var id = this.member[1]
        if(id == "-") id = this.member[2]
      }
      if(this.modalRank)
      {
        mp.trigger("changefamilyrank", id, parseInt(this.changeRank));
      }
      if(this.activeDelete)
      {
        mp.trigger("kickfamilymember", id, this.reasonKick);
      }
      if(this.activeDisbandModal)
      {
        mp.trigger("disbandFamily", this.reasonKick);
      }
      this.closeMenu()
    },
    saveSettings() {
      if(this.activeChangeRank)
      {
        console.log(JSON.stringify(this.allranks));
        mp.trigger("saveChangesRanks", JSON.stringify(this.allranks));
      }
      else {
        mp.trigger("saveFamilySettings", this.newdesc_1, this.newdesc_2);
      }
      this.closeMenu();
    },
    rankSettings() {
      this.activeChangeRank = true;
    },
    disbandFamily() {
      this.activeDisbandModal = true;
    },
    cancelmodal() {
      this.activeModal = false
      this.activeChangeRank = false
      this.activeDisbandModal = false
      this.modalRank = false
    },
    closeMenu() {
      this.active = false
      this.page = 0
      this.resetData()
      mp.trigger("closeFamilyManagerMenu")
    },
    resetData(){
      this.member = null
      this.changeRank = null
      this.modalRank = false
      this.activeChangeRank = false
      this.activeDisbandModal = false
      this.activeModal = false
      this.reasonKick = null
      this.invitePlayerID = null
    }
  }
})
