<template>
  <div
    class="big-data-grid"
    v-if="getLoggedInUser.userType === userTypeEnum.TEAM_MANAGEMENT"
  >
    <div class="row">
      <div class="col">
        <h3>Transfery zawodników</h3>
      </div>
      <div class="col text-right">
        <DxButton
          text="Złóż ofertę transferową"
          @click="showAddPlayerTransferOfferPopup()"
          type="default"
        />
      </div>
    </div>
    <DxTabPanel
      @title-click="tabChanged"
      v-model:selected-index="selectedIndex"
      v-if="userTeamId"
    >
      <DxItem title="Otrzymane oferty transferowe">
        <div>
          <ToTeamTransferOffersList 
            :userTeamId="userTeamId"
          />
        </div>
      </DxItem>
      <DxItem title="Złożone oferty transferowe">
        <div>
          <FromTeamTransferOffersList />
        </div>
      </DxItem>
    </DxTabPanel>
  </div>
  <PlayerTransferOfferAdd
    v-if="playerTransferOfferAddPopupOptions.isVisible"
    @closed="onPlayerTransferOfferPopupClose"
  />
</template>
<script>
import DxTabPanel, { DxItem } from "devextreme-vue/tab-panel";
import DxButton from "devextreme-vue/button";
import { mapGetters, mapMutations, mapActions } from "vuex";
import { userTypeEnum } from "../../enums/userTypeEnum";
import ToTeamTransferOffersList from "./ToTeamTransferOffersList";
import FromTeamTransferOffersList from "./FromTeamTransferOffersList";
import PlayerTransferOfferAdd from "./PlayerTransferOfferAdd";

export default {
  name: "PlayerTransferOffers",
  data() {
    return {
      selectedIndex: 0,
      userTypeEnum,
      userTeamId: null,
      playerTransferOfferAddPopupOptions: {
        isVisible: false,
      },
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
    }),
  },
  methods: {
    ...mapActions({
      setToTeamTransferOffers: "playerTransferOffers/setToTeamTransferOffers",
      setFromTeamTransferOffers:
        "playerTransferOffers/setFromTeamTransferOffers",
      getUserTeamId: "authentication/getUserTeamId",
    }),
    ...mapMutations({
      RESET_PLAYER_TRANSFER_OFFERS:
        "playerTransferOffers/RESET_PLAYER_TRANSFER_OFFERS",
    }),
    showAddPlayerTransferOfferPopup() {
      this.playerTransferOfferAddPopupOptions.isVisible = true;
    },
    onPlayerTransferOfferPopupClose() {
      this.playerTransferOfferAddPopupOptions.isVisible = false;
    },
    tabChanged() {
      this.RESET_PLAYER_TRANSFER_OFFERS();
      switch (this.selectedIndex) {
        case 0:
          this.setToTeamTransferOffers(this.userTeamId);
          break;
        case 1:
          this.setFromTeamTransferOffers(this.userTeamId);
          break;
      }
    },
  },
  mounted() {
    this.getUserTeamId().then((response) => {
      this.userTeamId = response.data;
      this.setToTeamTransferOffers(response.data);
    });
  },
  components: {
    DxTabPanel,
    DxItem,
    DxButton,
    ToTeamTransferOffersList,
    FromTeamTransferOffersList,
    PlayerTransferOfferAdd,
  },
  beforeUnmount() {
    this.RESET_PLAYER_TRANSFER_OFFERS();
  },
};
</script>
