<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      width="50%"
      height="90%"
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
      <DxScrollView width="100%" height="100%">
        <div class="m-3 text-center">
          <div class="line">
            <div class="row">
              <h3 class="col">
                <router-link
                  :to="{ name: 'teamDetails', params: { id: HomeTeam.id } }"
                  >{{ HomeTeam.name }}</router-link
                >
              </h3>
              <h3 class="col">
                {{ matchResult }}
              </h3>
              <h3 class="col">
                <router-link
                  :to="{ name: 'teamDetails', params: { id: AwayTeam.id } }"
                  >{{ AwayTeam.name }}</router-link
                >
              </h3>
            </div>
            <div class="row mb-2">
              <div class="col">
                <router-link
                  :to="{ name: 'teamDetails', params: { id: HomeTeam.id } }"
                  ><img
                    :src="HomeTeam.image"
                    alt="Herb drużyny domowej"
                    class="avatar"
                /></router-link>
              </div>
              <div class="col"></div>
              <div class="col">
                <router-link
                  :to="{ name: 'teamDetails', params: { id: AwayTeam.id } }"
                  ><img
                    :src="AwayTeam.image"
                    alt="Herb drużyny wyjazdowej"
                    class="avatar"
                /></router-link>
              </div>
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
          <div class="row" v-if="matchTypes">
            <div class="col">
              {{ matchTypes[MatchType].name }}
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
          <div class="line mt-4" />
          <h4>Składy</h4>
          <div class="row mt-4 pitch">
            <div class="container">
              <div class="col text-center">
                <div class="row mt-3">
                  <div class="col circle-center">
                    <div class="player">
                      {{ HomeMatchGoalkeeper.number }}
                    </div>
                    <span class="text-player">
                      {{ HomeMatchGoalkeeper.lastName }}
                    </span>
                  </div>
                </div>
                <div class="row mt-4">
                  <div
                    class="col circle-center"
                    v-for="defender in HomeMatchDefenders"
                    v-bind:key="defender"
                  >
                    <div class="player">
                      {{ defender.number }}
                    </div>
                    <span class="text-player">
                      {{ defender.lastName }}
                    </span>
                  </div>
                </div>
                <div class="row mt-4">
                  <div
                    class="col circle-center"
                    v-for="midfielder in HomeMatchMidfielders"
                    v-bind:key="midfielder"
                  >
                    <div class="player">
                      {{ midfielder.number }}
                    </div>
                    <span class="text-player">
                      {{ midfielder.lastName }}
                    </span>
                  </div>
                </div>
                <div class="row mt-4">
                  <div
                    class="col circle-center"
                    v-for="striker in HomeMatchStrikers"
                    v-bind:key="striker"
                  >
                    <div class="player">
                      {{ striker.number }}
                    </div>
                    <span class="text-player">
                      {{ striker.lastName }}
                    </span>
                  </div>
                </div>
                <div class="mt-3">
                  &nbsp;
                </div>
                <div class="row mt-4">
                  <div
                    class="col circle-center"
                    v-for="striker in AwayMatchStrikers"
                    v-bind:key="striker"
                  >
                    <div class="player">
                      {{ striker.number }}
                    </div>
                    <span class="text-player">
                      {{ striker.lastName }}
                    </span>
                  </div>
                </div>
                <div class="row mt-4">
                  <div
                    class="col circle-center"
                    v-for="midfielder in AwayMatchMidfielders"
                    v-bind:key="midfielder"
                  >
                    <div class="player">
                      {{ midfielder.number }}
                    </div>
                    <span class="text-player">
                      {{ midfielder.lastName }}
                    </span>
                  </div>
                </div>
                <div class="row mt-4">
                  <div
                    class="col circle-center"
                    v-for="defender in AwayMatchDefenders"
                    v-bind:key="defender"
                  >
                    <div class="player">
                      {{ defender.number }}
                    </div>
                    <span class="text-player">
                      {{ defender.lastName }}
                    </span>
                  </div>
                </div>
                <div class="row mt-4 mb-3">
                  <div class="col circle-center">
                    <div class="player">
                      {{ AwayMatchGoalkeeper.number }}
                    </div>
                    <span class="text-player">
                      {{ AwayMatchGoalkeeper.lastName }}
                    </span>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="line mt-4" />
          <h4>Przebieg meczu</h4>
          <div v-for="event in MatchEvents" v-bind:key="event.id">
            <div class="row">
              <div class="col" v-if="event.teamId === AwayTeamId" />
              <div class="col" v-if="event.matchEventType === 1">
                <i class="fas fa-futbol ico main-color"></i> {{ event.minute }}'
                {{ event.lastName }}
                <div v-if="event.assistPlayerLastName">
                  <i class="far fa-handshake ico main-color"></i>
                  {{ event.assistPlayerLastName }}
                </div>
              </div>
              <div class="col" v-if="event.matchEventType === 2">
                <span v-if="event.penaltyType === 0">
                  <i class="fas fa-exclamation ico main-color"></i>
                  {{ event.minute }}' {{ event.lastName }}
                </span>
                <span v-if="event.penaltyType === 1">
                  <i class="fas fa-square ico yellow-color"></i>
                  {{ event.minute }}' {{ event.lastName }}
                </span>
                <span v-if="event.penaltyType === 2">
                  <i class="fas fa-square ico red-color"></i>
                  {{ event.minute }}'
                  {{ event.lastName }}
                </span>
              </div>
              <div class="col" v-if="event.teamId === HomeTeamId" />
            </div>
          </div>
        </div>
      </DxScrollView>
    </DxPopup>
  </div>
</template>
<script>
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import { mapActions, mapMutations } from "vuex";
import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "matches/getField",
  mutationType: "matches/updateField",
});
import { useToast } from "vue-toastification";
import { DxScrollView } from "devextreme-vue/scroll-view";

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
      matchTypes: [],
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
      "match.MatchType",
      "match.DateTime",
      "match.MatchEvents",
      "match.HomeMatchGoalkeeper",
      "match.HomeMatchDefenders",
      "match.HomeMatchMidfielders",
      "match.HomeMatchStrikers",
      "match.AwayMatchGoalkeeper",
      "match.AwayMatchDefenders",
      "match.AwayMatchMidfielders",
      "match.AwayMatchStrikers",
    ]),
    getFormattedDateTime() {
      var tmpDate = new Date(this.DateTime);
      return tmpDate.toLocaleString();
    },
  },
  methods: {
    ...mapActions({
      setMatchDetails: "matches/setMatchDetails",
      getMatchTypesLookup: "matches/getMatchTypesLookup",
      sendMatchAnswer: "matches/sendMatchAnswer",
      getUserTeamId: "authentication/getUserTeamId",
    }),
    ...mapMutations({
      RESET_MATCH_FORM: "matches/RESET_MATCH_FORM",
    }),
    async handleSubmit(isAccepted) {
      await this.sendMatchAnswer({
        isAccepted: isAccepted,
        teamId: this.userTeamId,
      });
      useToast().success("Odpowiedź na mecz została wysłana pomyślnie!");
      this.popupVisible = false;
      this.$emit("closed");
    },
  },
  components: {
    DxPopup,
    DxToolbarItem,
    DxScrollView,
  },
  mounted() {
    this.getMatchTypesLookup().then((response) => {
      this.matchTypes = response.data;
    });
    this.getUserTeamId().then((response) => {
      this.userTeamId = response.data;
    });
    this.popupVisible = true;
    this.setMatchDetails(this.matchId);
  },
  beforeUnmount() {
    this.RESET_MATCH_FORM();
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
.avatar {
  width: 120px;
  height: 120px;
}
.pitch {
  background: #26a65b;
  width: 30vw;
  height: 100%;
  border-radius: 5px;
  padding: 20px;
  margin: 0px;
  display: inline-block;
}
.pitch .container {
  width: 95%;
  height: 99%;
  border: 2px solid #eee;
  padding: 0px;
}
.player {
  text-align: center;
  width: 2vw;
  height: 2vw;
  background: #eee;
  font-weight: bold;
  font-size: 120%;
  color: #2f363c;
  border: 2px solid #2f363c;
  padding: 8px;
  border-radius: 50%;
}
.text-player {
  background: #2f363c;
  padding: 4px 6px;
  border-radius: 20px;
}
.circle-center {
  text-align: -webkit-center;
  cursor: pointer;
}
</style>
