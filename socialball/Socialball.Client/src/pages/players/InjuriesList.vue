<template>
  <div class="big-data-grid">
    <div class="row">
      <div class="col">
        <h3>Zarządzanie drużyną</h3>
      </div>
      <!-- <div class="col text-right">
        <DxButton
          text="Dodaj nową kontuzję w drużynie"
          @click="showAddPopup"
          type="default"
        />
      </div> -->
    </div>
    <DxDataGrid
      :data-source="getPlayers"
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
      <DxColumn data-field="firstName" caption="Imię" data-type="string" />
      <DxColumn data-field="lastName" caption="Nazwisko" data-type="string" />
      <DxColumn data-field="position" caption="Pozycja" data-type="number">
        <DxLookup
          :data-source="positions"
          value-expr="value"
          display-expr="name"
        />
      </DxColumn>
      <DxColumn data-field="teamId" caption="Klub" data-type="string">
        <DxLookup :data-source="getTeams" value-expr="id" display-expr="name" />
      </DxColumn>
      <DxColumn
        data-field="citizenship"
        caption="Narodowość"
        data-type="string"
      />
      <DxColumn
        data-field="isInjuredUntil"
        caption="Kontuzjowany do:"
        data-type="date"
        format="dd/MM/yyyy"
        :editorOptions="{ showClearButton: true }"
      />
    </DxDataGrid>
  </div>

  <!-- Add popup
  <AddEditInjury
    :showToEdit="false"
    v-if="addPopupOptions.isVisible"
    @closed="onAddPopupClose"
  /> -->

  <!-- Edit popup -->
  <AddEditInjury
    :showToEdit="true"
    :userId="editPopupOptions.userId"
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

import { mapGetters, mapActions, mapMutations } from "vuex";
import AddEditInjury from "./AddEditInjury";

export default {
  name: "InjuriesList",
  data() {
    return {
      positions: [],
      // addPopupOptions: {
      //   isVisible: false,
      // },
      editPopupOptions: {
        isVisible: false,
        userId: "",
      },
    };
  },
  computed: {
    ...mapGetters({
      getPlayers: "players/getPlayers",
      getTeams: "teams/getTeams",
      isLoggedIn: "authentication/isLoggedIn",
    }),
  },
  methods: {
    ...mapActions({
      setPlayers: "players/setPlayers",
      setAllTeams: "teams/setAllTeams",
      setPositionsToLookup: "teams/setPositionsToLookup",
      getUserTeamId: "authentication/getUserTeamId",
    }),
    ...mapMutations({
      RESET_PLAYERS: "players/RESET_PLAYERS",
    }),
    onRowClick(e) {
      this.$router.push({ path: `/players/${e.data.id}` });
    },
    showAddPopup() {
      this.addPopupOptions.isVisible = true;
    },
    showEditPopup(e) {
      this.editPopupOptions.isVisible = true;
      this.editPopupOptions.userId = e.data.id;
    },
    onAddPopupClose() {
      this.addPopupOptions.isVisible = false;
    },
    onEditPopupClose() {
      this.editPopupOptions.isVisible = false;
      this.editPopupOptions.userId = "";
    },
  },
  mounted() {
    this.setPositionsToLookup().then((response) => {
      this.positions = response.data;
    });
    this.getUserTeamId().then((response) => {
      this.userTeamId = response.data;
      this.setPlayers(response.data);
    });
    this.setAllTeams();
  },
  components: {
    DxDataGrid,
    DxLoadPanel,
    DxColumn,
    DxFilterRow,
    DxLookup,
    AddEditInjury,
  },
  beforeUnmount() {
    this.RESET_PLAYERS();
  },
};
</script>
