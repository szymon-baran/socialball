<template>
  <div class="big-data-grid">
    <h3>Zarządzanie drużynami (Administrator)</h3>
    <div class="row">
      <div class="col-3">
        <DxSelectBox
          :dataSource="leagues"
          value-expr="id"
          display-expr="name"
          v-model="chosenLeagueId"
          id="leagueSelectBox"
          placeholder="Wybierz ligę"
          @value-changed="onLeagueChanged"
        />
      </div>
    </div>
    <div class="row mt-4">
      <div class="col">
        <DxDataGrid
          :data-source="getTeams"
          :remote-operations="false"
          :row-alternation-enabled="true"
          :show-borders="true"
          :hover-state-enabled="true"
          @row-click="showEditPopup"
          :column-auto-width="true"
          width="100%"
          no-data-text="Najpierw wybierz ligę"
        >
          <DxFilterRow :visible="true" />
          <DxLoadPanel :enabled="true" />
          <DxSorting mode="none" />
          <DxColumn data-field="name" caption="Nazwa drużyny" />
          <DxColumn data-field="isActive" caption="Czy drużyna jest aktywna?" />
          <DxColumn
            data-field="points"
            caption="Liczba punktów"
            :sort-index="0"
            sort-order="desc"
          />
        </DxDataGrid>
      </div>
    </div>
  </div>
  <AdminTeamEdit
    :showToEdit="true"
    :teamId="editPopupOptions.teamId"
    :selectedLeagueId="chosenLeagueId"
    v-if="editPopupOptions.isVisible"
    @closed="onEditPopupClose"
  />
</template>

<script>
import {
  DxDataGrid,
  DxLoadPanel,
  DxSorting,
  DxColumn,
  DxFilterRow,
} from "devextreme-vue/data-grid";
import DxSelectBox from "devextreme-vue/select-box";
import { mapGetters, mapActions, mapMutations } from "vuex";
import AdminTeamEdit from "./AdminTeamEdit";

export default {
  name: "AdminTeamsList",
  data() {
    return {
      leagues: [],
      chosenLeagueId: null,
      editPopupOptions: {
        isVisible: false,
        userId: "",
      },
    };
  },
  computed: {
    ...mapGetters({
      getTeams: "teams/getTeams",
    }),
  },
  methods: {
    ...mapActions({
      setLeaguesToLookup: "teams/setLeaguesToLookup",
      setTeamsByLeagueAdmin: "teams/setTeamsByLeagueAdmin",
    }),
    ...mapMutations({
      RESET_TEAMS: "teams/RESET_TEAMS",
    }),

    onLeagueChanged(e) {
      if (this.chosenLeagueId != null) {
        this.setTeamsByLeagueAdmin(e.value);
      }
    },

    showEditPopup(e) {
      this.editPopupOptions.isVisible = true;
      this.editPopupOptions.teamId = e.data.id;
    },

    onEditPopupClose() {
      this.editPopupOptions.isVisible = false;
      this.editPopupOptions.teamId = "";
    },
  },
  mounted() {
    this.RESET_TEAMS();
    this.chosenLeagueId = null;
    this.setLeaguesToLookup().then((response) => {
      this.leagues = response.data;
    });
  },
  components: {
    DxDataGrid,
    DxLoadPanel,
    DxSorting,
    DxColumn,
    DxFilterRow,
    DxSelectBox,
    AdminTeamEdit,
  },
  beforeUnmount() {
    this.RESET_TEAMS();
    this.chosenLeagueId = null;
  },
};
</script>
