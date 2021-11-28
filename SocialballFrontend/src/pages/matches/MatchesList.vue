<template>
  <div v-bind:class="{ 'big-data-grid': !teamId }">
    <div class="row mb-2">
      <div class="col">
        <h3 v-if="!teamId">Lista wszystkich meczy</h3>
      </div>
    </div>
    <DxDataGrid
      :data-source="getMatches"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
      :hover-state-enabled="true"
      key-expr="id"
      parent-id-expr="matchId"
      @row-click="showMatchDetails"
      :column-auto-width="true"
      width="100%"
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
    <div class="row mt-2">
      <div class="col text-right">
        <p
          v-if="
            getLoggedInUser &&
              getLoggedInUser.userType === userTypeEnum.TEAM_MANAGEMENT &&
              userTeamId === teamId
          "
        >
          Aby dodać lub zaakceptować mecz, przejdź na stronę zarządzania w
          profilu lub kliknij
          <router-link to="/unconfirmed-matches">tutaj</router-link>.
        </p>
      </div>
    </div>
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
import { userTypeEnum } from "../../enums/userTypeEnum";

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
      addMatchPopupOptions: {
        isVisible: false,
      },
      userTypeEnum,
      userTeamId: null,
    };
  },
  computed: {
    ...mapGetters({
      getMatches: "matches/getMatches",
      getTeams: "teams/getTeams",
      getLoggedInUser: "authentication/getLoggedInUser",
    }),
  },
  methods: {
    ...mapActions({
      setMatches: "matches/setMatches",
      setAllTeams: "teams/setAllTeams",
      getUserTeamId: "authentication/getUserTeamId",
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
    showAddMatchPopup() {
      this.addMatchPopupOptions.isVisible = true;
    },
    onAddMatchPopupClose() {
      this.addMatchPopupOptions.isVisible = false;
    },
  },
  mounted() {
    if (this.getLoggedInUser) {
      this.getUserTeamId().then((response) => {
        this.userTeamId = response.data;
      });
    }
    this.setMatches(this.$route.params.id);
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
