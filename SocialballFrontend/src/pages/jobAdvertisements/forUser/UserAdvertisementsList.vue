<template>
  <div style="min-height: 70vh;">
    <h3>Ogłoszenia drużyn, poszukujących zawodników na Twoją pozycję</h3>
    <DxDataGrid
      :data-source="getJobAdvertisements"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
      :hover-state-enabled="true"
      @row-click="showJobAdvertisementDetails"
    >
      <DxFilterRow :visible="true" />
      <DxLoadPanel :enabled="true" />
      <DxColumn data-field="teamId" caption="Drużyna">
        <DxLookup :data-source="teams" value-expr="id" display-expr="name" />
      </DxColumn>
      <DxColumn data-field="earnings" caption="Zarobki" />
      <DxColumn
        data-field="trainingSessionsPerWeek"
        caption="Liczba sesji treningowych"
      />
      <DxColumn data-field="location" caption="Lokalizacja" />
      <DxColumn
        data-field="isAlreadyAnswered"
        caption="Czy już odpowiedziałeś?"
        data-type="boolean"
      />
    </DxDataGrid>
  </div>
  <JobAdvertisementAnswerAdd
    :jobAdvertisementId="detailsPopupOptions.jobAdvertisementId"
    :jobAdvertisementAnswerType="detailsPopupOptions.jobAdvertisementAnswerType"
    :isAlreadyAnswered="detailsPopupOptions.isAlreadyAnswered"
    v-if="detailsPopupOptions.isVisible"
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
import { mapGetters, mapActions, mapMutations } from "vuex";
import JobAdvertisementAnswerAdd from "../JobAdvertisementAnswerAdd";
import { jobAdvertisementTypeEnum } from "../../../enums/jobAdvertisementTypeEnum";

export default {
  name: "UserAdvertisementsList",
  data() {
    return {
      jobAdvertisementTypeEnum,
      teams: [],
      detailsPopupOptions: {
        isVisible: false,
        jobAdvertisementId: "",
        jobAdvertisementAnswerType: null,
        isAlreadyAnswered: false,
      },
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
      getJobAdvertisements: "jobAdvertisements/getJobAdvertisements",
    }),
  },
  methods: {
    ...mapActions({
      getTeamsToLookup: "jobAdvertisements/getTeamsToLookup",
      setUserAdvertisements: "jobAdvertisements/setUserAdvertisements",
    }),
    ...mapMutations({
      RESET_JOB_ADVERTISEMENTS: "jobAdvertisements/RESET_JOB_ADVERTISEMENTS",
    }),
    showJobAdvertisementDetails(e) {
      this.detailsPopupOptions.isVisible = true;
      this.detailsPopupOptions.jobAdvertisementId = e.data.id;
      this.detailsPopupOptions.jobAdvertisementAnswerType =
        jobAdvertisementTypeEnum.FROM_USER;
      this.detailsPopupOptions.isAlreadyAnswered = e.data.isAlreadyAnswered;
    },
    onDetailsPopupClose() {
      this.detailsPopupOptions.isVisible = false;
      this.detailsPopupOptions.jobAdvertisementId = "";
      this.detailsPopupOptions.jobAdvertisementAnswerType = null;
      this.detailsPopupOptions.isAlreadyAnswered = false;
      this.setUserAdvertisements(this.getLoggedInUser.id);
    },
  },
  mounted() {
    this.getTeamsToLookup().then((response) => {
      this.teams = response.data;
    });
    this.setUserAdvertisements(this.getLoggedInUser.id);
  },
  beforeUnmount() {
    this.RESET_JOB_ADVERTISEMENTS();
  },
  components: {
    DxDataGrid,
    DxLoadPanel,
    DxColumn,
    DxFilterRow,
    DxLookup,
    JobAdvertisementAnswerAdd,
  },
};
</script>
