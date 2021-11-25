<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      :width="650"
      :height="800"
      container=".dx-viewport"
      title="Szczegóły meczu"
      :shading="false"
      v-if="Id"
    >
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="before"
        :options="closeButtonOptions"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="sendButtonOptionsReject"
        v-if="isUnconfirmedByYourTeam"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="sendButtonOptionsConfirm"
        v-if="isUnconfirmedByYourTeam"
      />
      <div class="m-3 text-center">
        <div class="line">
          <div class="row">
            <h3 class="col header-font">
              {{ HomeTeam.name }}
            </h3>
            <h3 class="col header-font">
              {{ matchResult }}
            </h3>
            <h3 class="col header-font">
              {{ AwayTeam.name }}
            </h3>
          </div>
        </div>
        <div class="row">
          <div class="col">
            {{ Stadium }}
          </div>
        </div>
        <div class="row">
          <div class="col">
            {{ getFormattedDateTime }}
          </div>
        </div>
        <div class="row">
          <div class="col">
            herb1
          </div>
          <div class="col">
            herb2
          </div>
        </div>
        <div class="row">
          <div class="col" v-if="HomeTeam.league">
            {{ HomeTeam.league.name }}
          </div>
          <div class="col" v-if="AwayTeam.league">
            {{ AwayTeam.league.name }}
          </div>
        </div>
        <div class="line" />
        <h4>Przebieg meczu</h4>
        <div v-for="event in MatchEvents" v-bind:key="event.id">
          <div class="row">
            <div class="col" v-if="event.player.teamId === AwayTeamId" />
            <div class="col" v-if="event.matchEventType === 1">
              <i class="fas fa-futbol ico main-color"></i> {{ event.minute }}'
              {{ event.player.lastName }}
              <div v-if="event.assistPlayerId">
                <i class="far fa-handshake ico main-color"></i>
                {{ event.assistPlayer.lastName }}
              </div>
            </div>
            <div class="col" v-if="event.matchEventType === 2">
              <span v-if="event.penaltyType === 0">
                <i class="fas fa-exclamation ico main-color"></i>
                {{ event.minute }}' {{ event.player.lastName }}
              </span>
              <span v-if="event.penaltyType === 1">
                <i class="fas fa-square ico yellow-color"></i>
                {{ event.minute }}' {{ event.player.lastName }}
              </span>
              <span v-if="event.penaltyType === 2">
                <i class="fas fa-square ico red-color"></i> {{ event.minute }}'
                {{ event.player.lastName }}
              </span>
            </div>
            <div class="col" v-if="event.player.teamId === HomeTeamId" />
          </div>
        </div>
      </div>
    </DxPopup>
  </div>
</template>
<script>
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import { mapActions } from "vuex";
import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "matches/getField",
  mutationType: "matches/updateField",
});
import { useToast } from "vue-toastification";

export default {
  name: "MatchDetails",
  props: {
    matchId: {
      type: String,
      required: true,
    },
    matchResult: {
      type: String,
      default: "",
    },
    isUnconfirmedByYourTeam: {
      type: Boolean,
      required: false,
    },
  },
  data() {
    return {
      popupVisible: true,
      closeButtonOptions: {
        text: "Zamknij",
        onClick: () => {
          this.popupVisible = false;
          this.$emit("closed");
        },
        type: "normal",
      },
      sendButtonOptionsConfirm: {
        icon: "check",
        text: "Potwierdź",
        onClick: () => {
          this.handleSubmit(true);
        },
      },
      sendButtonOptionsReject: {
        icon: "remove",
        text: "Odrzuć",
        type: "danger",
        onClick: () => {
          this.handleSubmit(false);
        },
      },
      userTeamId: null,
    };
  },
  computed: {
    ...mapFields([
      "match.Id",
      "match.HomeTeamId",
      "match.HomeTeam",
      "match.AwayTeamId",
      "match.AwayTeam",
      "match.Stadium",
      "match.DateTime",
      "match.MatchEvents",
    ]),
    getFormattedDateTime() {
      var tmpDate = new Date(this.DateTime);
      return tmpDate.toLocaleString();
    },
  },
  methods: {
    ...mapActions({
      setMatchDetails: "matches/setMatchDetails",
      sendMatchAnswer: "matches/sendMatchAnswer",
      getUserTeamId: "authentication/getUserTeamId",
    }),
    async handleSubmit(isAccepted) {
      await this.sendMatchAnswer({ isAccepted: isAccepted, teamId: this.userTeamId });
      useToast().success("Odpowiedź na mecz została wysłana pomyślnie!");
      this.popupVisible = false;
      this.$emit("closed");
    },
  },
  components: {
    DxPopup,
    DxToolbarItem,
  },
  mounted() {
    this.getUserTeamId().then((response) => {
      this.userTeamId = response.data;
    });
    this.popupVisible = true;
    this.setMatchDetails(this.matchId);
  },
};
</script>
<style scoped>
h2 {
  font-size: 220%;
}
h3 {
  font-size: 200%;
}
h4 {
  font-size: 160%;
  margin-bottom: 3vh;
}
.ico {
  font-size: 120%;
  margin: 0px 2px;
}
</style>
