<template>
  <div style="min-height: 70vh;">
    <DxDataGrid
      :data-source="getPlayerTransferOffers"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
      :hover-state-enabled="true"
      :column-auto-width="true"
      width="100%"
      @row-click="showDetailsPopup"
    >
      <DxFilterRow :visible="true" />
      <DxLoadPanel :enabled="true" />
      <DxColumn data-field="fromTeamId" caption="Oferta od">
        <DxLookup :data-source="teams" value-expr="id" display-expr="name" />
      </DxColumn>
      <DxColumn data-field="transferFee" caption="Kwota transferu [PLN]" />
      <DxColumn data-field="isAcceptedByOtherTeam" caption="Czy zaakceptowane przez Twoją drużynę?" />
    </DxDataGrid>
  </div>
  <PlayerTransferOfferDetails
    v-if="detailsPopupOptions.isVisible"
    :showAsTeam="false"
    @closed="onDetailsPopupClose"
  />
</template>
<script>
import {
  DxDataGrid,
  DxLoadPanel,
  DxColumn,
  DxFilterRow,
  DxLookup,
} from "devextreme-vue/data-grid";
import { mapGetters, mapActions } from "vuex";
import PlayerTransferOfferDetails from "./PlayerTransferOfferDetails";

export default {
  name: "ToPlayerTransferOffersList",
  data() {
    return {
      teams: [],
      detailsPopupOptions: {
        isVisible: false,
      },
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
      getPlayerTransferOffers: "playerTransferOffers/getPlayerTransferOffers",
    }),
  },
  methods: {
    ...mapActions({
      setPlayerTransferOffers: "playerTransferOffers/setPlayerTransferOffers",
      getPlayersByTeam: "matches/getPlayersByTeam",
      setTeamsToLookup: "teams/setTeamsToLookup",
      setPlayerTransferOfferDetails:
        "playerTransferOffers/setPlayerTransferOfferDetails",
    }),
    showDetailsPopup(e) {
      this.detailsPopupOptions.isVisible = true;
      this.setPlayerTransferOfferDetails(e.data.id);
    },
    onDetailsPopupClose() {
      this.detailsPopupOptions.isVisible = false;
    },
  },
  mounted() {
    this.setPlayerTransferOffers(this.getLoggedInUser.id);
    this.setTeamsToLookup().then((response) => {
      this.teams = response.data;
    });
  },
  components: {
    DxDataGrid,
    DxLoadPanel,
    DxColumn,
    DxFilterRow,
    DxLookup,
    PlayerTransferOfferDetails,
  },
};
</script>
