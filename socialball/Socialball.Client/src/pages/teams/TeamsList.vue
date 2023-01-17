<template>
  <div class="big-data-grid">
    <h3>Tabele ligowe</h3>
    <div class="row">
      <div class="col-3">
        <DxSelectBox
          :dataSource="leagues"
          value-expr="id"
          display-expr="name"
          v-model="ChosenLeagueId"
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
          @row-click="onRowClick"
          :column-auto-width="true"
          width="100%"
          no-data-text="Najpierw wybierz ligę"
        >
          <DxFilterRow :visible="true" />
          <DxLoadPanel :enabled="true" />
          <DxSorting mode="none" />
          <DxColumn data-field="name" caption="Nazwa drużyny" />
          <!-- <DxColumn data-field="leagueId" caption="Nazwa ligi">
            <DxLookup
              :data-source="leagues"
              value-expr="id"
              display-expr="name"
            />
          </DxColumn> -->
          <DxColumn data-field="league.country" caption="Kraj" />
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
</template>

<script>
import {
  DxDataGrid,
  DxLoadPanel,
  DxSorting,
  DxColumn,
  DxFilterRow,
  // DxLookup,
} from "devextreme-vue/data-grid";
import DxSelectBox from "devextreme-vue/select-box";
import { mapGetters, mapActions, mapMutations } from "vuex";

export default {
  name: "TeamsList",
  data() {
    return {
      leagues: [],
      ChosenLeagueId: null,
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
      setTeamsByLeague: "teams/setTeamsByLeague",
    }),
    ...mapMutations({
      RESET_TEAMS: "teams/RESET_TEAMS",
    }),

    onRowClick(e) {
      this.$router.push({
        name: "teamDetails",
        params: {
          id: e.data.id,
        },
      });
    },

    onLeagueChanged(e) {
      if (this.ChosenLeagueId != null) {
        this.setTeamsByLeague(e.value);
      }
    },
  },
  mounted() {
    this.RESET_TEAMS();
    this.ChosenLeagueId = null;
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
    // DxLookup,
    DxSelectBox,
  },
  beforeUnmount() {
    this.RESET_TEAMS();
    this.ChosenLeagueId = null;
  },
};
</script>
