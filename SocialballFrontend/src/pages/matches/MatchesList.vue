<template>
  <div v-bind:class="{ 'big-data-grid': !teamId }">
    <h3 v-if="!teamId">Lista wszystkich meczy</h3>
    <DxDataGrid
      :data-source="getMatches"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
      :hover-state-enabled="true"
      key-expr="id"
      parent-id-expr="matchId"
      @row-click="showMatchDetails"
    >
      <DxFilterRow :visible="true" />
      <DxLoadPanel :enabled="true" />
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
        caption="Data meczu"
        data-type="date"
        format="dd/MM/yyyy"
        :editorOptions="{ showClearButton: true }"
      />
    </DxDataGrid>

    <MatchDetailsPopup
      :matchId="detailsPopupOptions.selectedMatchId"
      :matchResult="detailsPopupOptions.selectedMatchResult"
      v-if="detailsPopupOptions.isVisible"
      @closed="onPopupClosed()"
    />
  </div>
</template>

<script>
import {
  DxDataGrid,
  DxLoadPanel,
  DxColumn,
  DxLookup,
  DxFilterRow,
} from "devextreme-vue/data-grid";
import { mapGetters, mapActions, mapMutations } from "vuex";

import MatchDetailsPopup from "./MatchDetailsPopup";

export default {
  name: "Matches",
  props: {
    teamId: {
      type: String,
      required: false,
    },
  },
  data() {
    return {
      detailsPopupOptions: {
        isVisible: false,
        selectedMatchId: "",
        selectedMatchResult: "",
      },
    };
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
    showMatchDetails(e) {
      this.detailsPopupOptions.isVisible = true;
      this.detailsPopupOptions.selectedMatchId = e.data.id;
      this.detailsPopupOptions.selectedMatchResult = e.data.result;
    },
    onPopupClosed() {
      this.detailsPopupOptions.isVisible = false;
      this.detailsPopupOptions.selectedMatchId = "";
      this.detailsPopupOptions.selectedMatchResult = "";
    },
  },
  mounted() {
    this.setMatches(this.teamId);
    this.setAllTeams();
  },
  components: {
    DxDataGrid,
    DxLoadPanel,
    DxColumn,
    DxLookup,
    DxFilterRow,
    MatchDetailsPopup,
  },
  beforeUnmount() {
    this.RESET_MATCHES();
  },
};
</script>
