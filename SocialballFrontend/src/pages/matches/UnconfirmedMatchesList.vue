<template>
  <div
    class="big-data-grid"
    v-if="getLoggedInUser.userType === userTypeEnum.TEAM_MANAGEMENT"
  >
    <div class="row mb-2">
      <div class="col">
        <h3>Lista niepotwierdzonych meczy</h3>
        <p class="mt-2">
          Potwierdzone mecze znajdziesz na stronie szczegółów swojej drużyny.
        </p>
      </div>
      <div class="col text-right">
        <DxButton
          text="Dodaj nowy mecz"
          @click="showAddMatchPopup"
          type="default"
        />
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
      <DxColumn
        data-field="isUnconfirmedByYourTeam"
        caption="Niepotwierdzone przez Twoją drużynę"
        data-type="boolean"
      />
    </DxDataGrid>
    <MatchDetailsPopup
      :matchId="detailsPopupOptions.selectedMatchId"
      :matchResult="detailsPopupOptions.selectedMatchResult"
      :isUnconfirmedByYourTeam="detailsPopupOptions.isUnconfirmedByYourTeam"
      v-if="detailsPopupOptions.isVisible"
      @closed="onPopupClosed()"
    />
    <MatchAdd
      v-if="addMatchPopupOptions.isVisible"
      @close="onAddMatchPopupClose()"
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
import DxButton from "devextreme-vue/button";

import MatchDetailsPopup from "./MatchDetailsPopup";
import MatchAdd from "./MatchAdd";
import { userTypeEnum } from "../../enums/userTypeEnum";

export default {
  name: "UnconfirmedMatches",
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
      setUnconfirmedMatches: "matches/setUnconfirmedMatches",
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
      this.detailsPopupOptions.isUnconfirmedByYourTeam = e.data.isUnconfirmedByYourTeam;
    },
    onPopupClosed() {
      this.detailsPopupOptions.isVisible = false;
      this.detailsPopupOptions.selectedMatchId = "";
      this.detailsPopupOptions.selectedMatchResult = "";
      this.detailsPopupOptions.isUnconfirmedByYourTeam = false;
    },
    showAddMatchPopup() {
      this.addMatchPopupOptions.isVisible = true;
    },
    onAddMatchPopupClose() {
      this.addMatchPopupOptions.isVisible = false;
    },
  },
  mounted() {
    this.getUserTeamId().then((response) => {
      this.userTeamId = response.data;
      this.setUnconfirmedMatches(this.userTeamId);
    });
    this.setAllTeams();
  },
  components: {
    DxDataGrid,
    DxLoadPanel,
    DxColumn,
    DxLookup,
    DxFilterRow,
    MatchDetailsPopup,
    MatchAdd,
    DxButton,
  },
  beforeUnmount() {
    this.RESET_MATCHES();
  },
};
</script>
