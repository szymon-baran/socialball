<template>
  <div style="min-height: 70vh;">
    <div class="row">
      <div class="col">
        <h3>Zarządzaj ogłoszeniami swojej drużyny</h3>
      </div>
      <div class="col text-right">
        <DxButton
          text="Dodaj nowe ogłoszenie"
          @click="showAddPopup"
          type="default"
        />
      </div>
    </div>
    <DxDataGrid
      :data-source="getMyJobAdvertisements"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
      :hover-state-enabled="true"
      @row-click="showEditPopup"
      :column-auto-width="true"
      width="100%"
    >
      <DxFilterRow :visible="true" />
      <DxLoadPanel :enabled="true" />
      <DxColumn data-field="position" caption="Pozycja">
        <DxLookup
          :data-source="positions"
          value-expr="value"
          display-expr="name"
        />
      </DxColumn>
      <DxColumn data-field="location" caption="Lokalizacja" />
      <DxColumn
        data-field="isActive"
        caption="Czy aktywne?"
        data-type="boolean"
      />
    </DxDataGrid>
  </div>

  <!-- Add popup -->
  <AddEditMyTeamAdvertisement
    :showToEdit="false"
    v-if="addPopupOptions.isVisible"
    @closed="onAddPopupClose"
  />

  <!-- Edit popup -->
  <AddEditMyTeamAdvertisement
    :showToEdit="true"
    :jobAdvertisementId="editPopupOptions.jobAdvertisementId"
    v-if="editPopupOptions.isVisible"
    @closed="onEditPopupClose"
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
import DxButton from "devextreme-vue/button";
import { mapGetters, mapActions, mapMutations } from "vuex";
import { jobAdvertisementTypeEnum } from "../../../enums/jobAdvertisementTypeEnum";
import AddEditMyTeamAdvertisement from "./AddEditMyTeamAdvertisement";

export default {
  name: "MyTeamAdvertisementsList",
  data() {
    return {
      positions: [],
      jobAdvertisementTypeEnum,
      addPopupOptions: {
        isVisible: false,
      },
      editPopupOptions: {
        isVisible: false,
        jobAdvertisementId: "",
      },
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
      getMyJobAdvertisements: "jobAdvertisements/getMyJobAdvertisements",
    }),
  },
  methods: {
    ...mapActions({
      setMyTeamAdvertisements: "jobAdvertisements/setMyTeamAdvertisements",
      setPositionsToLookup: "jobAdvertisements/setPositionsToLookup",
    }),
    ...mapMutations({
      RESET_MY_JOB_ADVERTISEMENTS:
        "jobAdvertisements/RESET_MY_JOB_ADVERTISEMENTS",
    }),
    showAddPopup() {
      this.addPopupOptions.isVisible = true;
    },
    showEditPopup(e) {
      this.editPopupOptions.isVisible = true;
      this.editPopupOptions.jobAdvertisementId = e.data.id;
    },
    onAddPopupClose() {
      this.addPopupOptions.isVisible = false;
      this.addPopupOptions.jobAdvertisementId = "";
      this.setMyTeamAdvertisements(this.getLoggedInUser.id);
    },
    onEditPopupClose() {
      this.editPopupOptions.isVisible = false;
      this.editPopupOptions.jobAdvertisementId = "";
      this.setMyTeamAdvertisements(this.getLoggedInUser.id);
    },
  },
  mounted() {
    this.setPositionsToLookup().then((response) => {
      this.positions = response.data;
    });
    this.setMyTeamAdvertisements(this.getLoggedInUser.id);
  },
  beforeUnmount() {
    this.RESET_MY_JOB_ADVERTISEMENTS();
  },
  components: {
    DxDataGrid,
    DxLoadPanel,
    DxColumn,
    DxFilterRow,
    DxLookup,
    DxButton,
    AddEditMyTeamAdvertisement,
  },
};
</script>
