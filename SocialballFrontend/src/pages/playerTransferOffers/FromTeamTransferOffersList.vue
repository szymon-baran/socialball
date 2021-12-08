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
    >
      <DxFilterRow :visible="true" />
      <DxLoadPanel :enabled="true" />
      <DxColumn data-field="playerName" caption="Zawodnik" />
      <DxColumn data-field="toTeamId" caption="Oferta do">
        <DxLookup
          :data-source="teams"
          value-expr="id"
          display-expr="name"
        />
      </DxColumn>
      <DxColumn data-field="transferFee" caption="Kwota transferu [PLN]" />
      <DxColumn data-field="isAcceptedByOtherTeam" caption="Zaakceptowane przez drużynę" />
      <DxColumn data-field="isAcceptedByPlayer" caption="Zaakceptowane przez zawodnika" />
    </DxDataGrid>
  </div>
  <!-- <JobAdvertisementAnswerResponse
    :jobAdvertisementAnswerId="detailsPopupOptions.jobAdvertisementAnswerId"
    v-if="detailsPopupOptions.isVisible"
    @closed="onDetailsPopupClose"
  /> -->
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

export default {
  name: "FromTeamTransferOffersList",
  data() {
    return {
      teamPlayers: [],
      teams: [],
      detailsPopupOptions: {
        isVisible: false,
        jobAdvertisementAnswerId: "",
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
      setTeamsToLookup: "teams/setTeamsToLookup",
    }),
    // showJobAdvertisementAnswerDetails(e) {
    //   this.detailsPopupOptions.isVisible = true;
    //   this.detailsPopupOptions.jobAdvertisementAnswerId = e.data.id;
    // },
    // onDetailsPopupClose() {
    //   this.detailsPopupOptions.isVisible = false;
    //   this.detailsPopupOptions.jobAdvertisementAnswerId = "";
    //   this.setJobAdvertisementsAnswers(this.getLoggedInUser.id);
    // },
  },
  mounted() {
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
  },
};
</script>
