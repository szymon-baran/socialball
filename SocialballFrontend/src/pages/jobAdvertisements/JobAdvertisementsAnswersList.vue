<template>
  <div style="min-height: 70vh;">
    <h3>Odpowiedzi na Twoje ogłoszenia</h3>
    <DxDataGrid
      :data-source="getJobAdvertisementsAnswers"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
      :hover-state-enabled="true"
      @row-click="showJobAdvertisementAnswerDetails"
      :column-auto-width="true"
      width="100%"
    >
      <DxFilterRow :visible="true" />
      <DxLoadPanel :enabled="true" />
      <DxColumn
        data-field="isResponded"
        caption="Czy udzielono odpowiedzi zwrotnej?"
        data-type="boolean"
      />
      <DxColumn data-field="senderName" caption="Nadawca" />
    </DxDataGrid>
  </div>
  <JobAdvertisementAnswerResponse
    :jobAdvertisementAnswerId="detailsPopupOptions.jobAdvertisementAnswerId"
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
import JobAdvertisementAnswerResponse from "./JobAdvertisementAnswerResponse";

export default {
  name: "JobAdvertisementsAnswersList",
  data() {
    return {
      detailsPopupOptions: {
        isVisible: false,
        jobAdvertisementAnswerId: "",
      },
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
      getJobAdvertisementsAnswers: "jobAdvertisements/getJobAdvertisementsAnswers",
    }),
  },
  methods: {
    ...mapActions({
      setJobAdvertisementsAnswers: "jobAdvertisements/setJobAdvertisementsAnswers",
    }),
    ...mapMutations({
      RESET_JOB_ADVERTISEMENTS_ANSWERS: "jobAdvertisements/RESET_JOB_ADVERTISEMENTS_ANSWERS",
    }),
    showJobAdvertisementAnswerDetails(e) {
      this.detailsPopupOptions.isVisible = true;
      this.detailsPopupOptions.jobAdvertisementAnswerId = e.data.id;
    },
    onDetailsPopupClose() {
      this.detailsPopupOptions.isVisible = false;
      this.detailsPopupOptions.jobAdvertisementAnswerId = "";
      this.setJobAdvertisementsAnswers(this.getLoggedInUser.id);
    },
  },
  mounted() {
    this.setJobAdvertisementsAnswers(this.getLoggedInUser.id);
  },
  beforeUnmount() {
    this.RESET_JOB_ADVERTISEMENTS_ANSWERS();
  },
  components: {
    DxDataGrid,
    DxLoadPanel,
    DxColumn,
    DxFilterRow,
    JobAdvertisementAnswerResponse,
  },
};
</script>
