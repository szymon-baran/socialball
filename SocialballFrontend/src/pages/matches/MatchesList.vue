<template>
  <div>
    <DxDataGrid
      :data-source="getMatches"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
      :hover-state-enabled="true"
      key-expr="id"
      parent-id-expr="matchId"
    >
      <DxFilterRow :visible="true" />

      <DxColumn data-field="homeTeamId" caption="Drużyna domowa">
        <DxLookup :data-source="getTeams" value-expr="id" display-expr="name" />
      </DxColumn>
      <DxColumn data-field="result" caption="Wynik" alignment="center" />
      <DxColumn data-field="awayTeamId" caption="Drużyna wyjazdowa">
        <DxLookup :data-source="getTeams" value-expr="id" display-expr="name" />
      </DxColumn>
      <DxColumn data-field="stadium" caption="Stadion" />
      <DxColumn
        data-field="dateTime"
        caption="Data i czas rozpoczęcia"
        format="shortDate"
      />
    </DxDataGrid>
  </div>
</template>

<script>
import {
  DxDataGrid,
  DxColumn,
  DxLookup,
  DxFilterRow,
} from "devextreme-vue/data-grid";

import { mapGetters, mapActions, mapMutations } from "vuex";

export default {
  name: "Matches",
  props: {
    teamId: {
      type: String,
      required: false,
    },
  },
  computed: {
    ...mapGetters({
      getMatches: "matches/getMatches",
      getTeams: "teams/getTeams",
    }),
  },
  methods: {
    ...mapActions({
      setMatches: "matches/setMatches",
      setAllTeams: "teams/setAllTeams",
    }),
    ...mapMutations({
      RESET_MATCHES: "matches/RESET_MATCHES",
    }),
  },
  mounted() {
    this.setMatches(this.teamId);
    this.setAllTeams();
  },
  components: {
    DxDataGrid,
    DxColumn,
    DxLookup,
    DxFilterRow,
  },
  beforeUnmount() {
    this.RESET_MATCHES();
  },
};
</script>
