<template>
  <div class="big-data-grid">
    <h3>Lista wszystkich drużyn</h3>
    <DxDataGrid
      :data-source="getTeams"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
      :hover-state-enabled="true"
      @row-click="onRowClick"
    >
      <DxFilterRow :visible="true" />
      <DxLoadPanel :enabled="true" />
      <DxColumn data-field="name" caption="Nazwa drużyny" />
      <DxColumn data-field="leagueId" caption="Nazwa ligi">
        <DxLookup :data-source="leagues" value-expr="id" display-expr="name" />
      </DxColumn>
      <DxColumn data-field="league.country" caption="Kraj" />
    </DxDataGrid>
  </div>
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

export default {
  name: "TeamsList",
  data() {
    return {
      leagues: []
    }
  },
  computed: {
    ...mapGetters({
      getTeams: "teams/getTeams",
    }),
  },
  methods: {
    ...mapActions({
      setAllTeams: "teams/setAllTeams",
      setLeaguesToLookup: "teams/setLeaguesToLookup",
    }),
    ...mapMutations({
      RESET_TEAMS: "teams/RESET_TEAMS",
    }),

    onRowClick(e) {
      this.$router.push({
        path: `/teams/${e.data.id}`,
        params: {
          teamName: e.data.name,
        },
      });
    },
  },
  mounted() {
    this.setLeaguesToLookup().then((response) => {
      this.leagues = response.data;
    });

    this.setAllTeams();
  },
  components: {
    DxDataGrid,
    DxLoadPanel,
    DxColumn,
    DxFilterRow,
    DxLookup,
  },
  beforeUnmount() {
    this.RESET_TEAMS();
  },
};
</script>
