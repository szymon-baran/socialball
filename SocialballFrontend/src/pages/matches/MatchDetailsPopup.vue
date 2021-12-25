<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      width="50vw"
      height="90vh"
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
          <div>
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
          <DxTabPanel class="mt-4">
            <DxItem title="Informacje i przebieg meczu">
              <MatchDetailsInformations />
            </DxItem>
            <DxItem title="Składy drużyn">
              <MatchDetailsTeamLineups />
            </DxItem>
          </DxTabPanel>
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
import DxTabPanel, { DxItem } from "devextreme-vue/tab-panel";
import MatchDetailsInformations from "./MatchDetailsInformations";
import MatchDetailsTeamLineups from "./MatchDetailsTeamLineups";

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
      "match.MatchType",
      "match.DateTime",
      "match.MatchEvents",
    ]),
  },
  methods: {
    ...mapActions({
      setMatchDetails: "matches/setMatchDetails",
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
    DxTabPanel,
    DxItem,
    MatchDetailsInformations,
    MatchDetailsTeamLineups,
  },
  mounted() {
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
</style>
