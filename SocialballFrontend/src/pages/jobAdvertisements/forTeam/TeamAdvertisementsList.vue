<template>
  <div style="min-height: 70vh;">
    <h3>Ogłoszenia zawodników bez drużyny</h3>
    <DxDataGrid
      :data-source="getJobAdvertisements"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
      :hover-state-enabled="true"
      @row-click="showJobAdvertisementDetails"
      :column-auto-width="true"
      width="100%"
    >
      <DxFilterRow :visible="true" />
      <DxLoadPanel :enabled="true" />
      <DxColumn data-field="userFullName" caption="Imię i nazwisko" />
      <DxColumn data-field="username" caption="Nazwa użytkownika" />
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
} from "devextreme-vue/data-grid";
import { mapGetters, mapActions, mapMutations } from "vuex";
import { jobAdvertisementTypeEnum } from "../../../enums/jobAdvertisementTypeEnum";
import JobAdvertisementAnswerAdd from "../JobAdvertisementAnswerAdd";

export default {
  name: "TeamAdvertisementsList",
  data() {
    return {
      jobAdvertisementTypeEnum,
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
      setTeamAdvertisements: "jobAdvertisements/setTeamAdvertisements",
    }),
    ...mapMutations({
      RESET_JOB_ADVERTISEMENTS: "jobAdvertisements/RESET_JOB_ADVERTISEMENTS",
    }),
    showJobAdvertisementDetails(e) {
      this.detailsPopupOptions.isVisible = true;
      this.detailsPopupOptions.jobAdvertisementId = e.data.id;
      this.detailsPopupOptions.jobAdvertisementAnswerType =
        jobAdvertisementTypeEnum.FROM_TEAM;
      this.detailsPopupOptions.isAlreadyAnswered = e.data.isAlreadyAnswered;
    },
    onDetailsPopupClose() {
      this.detailsPopupOptions.isVisible = false;
      this.detailsPopupOptions.jobAdvertisementId = "";
      this.detailsPopupOptions.jobAdvertisementAnswerType = null;
      this.detailsPopupOptions.isAlreadyAnswered = false;
      this.setTeamAdvertisements(this.getLoggedInUser.id);
    },
  },
  mounted() {
    this.setTeamAdvertisements(this.getLoggedInUser.id);
  },
  beforeUnmount() {
    this.RESET_JOB_ADVERTISEMENTS();
  },
  components: {
    DxDataGrid,
    DxLoadPanel,
    DxColumn,
    DxFilterRow,
    JobAdvertisementAnswerAdd,
  },
};
</script>
