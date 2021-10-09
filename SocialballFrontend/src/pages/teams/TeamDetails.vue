<template>
  <div class="big-data-grid">
    <h3>Szczegóły drużyny</h3>
    <h4 class="line">Lista zawodników</h4>
    <PlayersList :isProfileView="true" :teamId="this.$route.params.id" />
    <div class="row">
      <div class="col mt-2">
        <h4 class="line">Rozkład pozycji zawodników w obrębie drużyny</h4>
        <DxPieChart
          id="pie"
          :data-source="getPositionsData"
          type="doughnut"
          palette="Material"
          v-if="positions.length > 0"
          class="mt-3"
        >
          <DxSeries argument-field="position" value-field="numberOfPlayers">
            <DxLabel :visible="true">
              <DxConnector :visible="true" />
            </DxLabel>
          </DxSeries>
          <DxLegend
            :margin="0"
            horizontal-alignment="right"
            vertical-alignment="top"
            :customize-text="customizeLegend"
          />
          <DxTooltip :enabled="true" :customize-tooltip="customizeTooltip" />
          <DxSize :width="450" />
        </DxPieChart>
      </div>
      <div class="col mt-2">
        <h4 class="line text-right">Terminarz zespołu na aktualny miesiąc</h4>
          <MatchesList 
            :teamId="this.$route.params.id"
          />
      </div>
    </div>
    <div class="mt-5">
      <DxButton text="Kontakt" @click="routerPushToContact" />
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import DxButton from "devextreme-vue/button";
import PlayersList from "../players/PlayersList.vue";
import DxPieChart, {
  DxLegend,
  DxSeries,
  DxLabel,
  DxTooltip,
  DxConnector,
  DxSize,
} from "devextreme-vue/pie-chart";
import MatchesList from "../matches/MatchesList.vue";

export default {
  name: "TeamDetails",
  params: {
    teamName: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      positions: [],
    };
  },
  computed: {
    ...mapGetters({
      getPositionsData: "teams/getPositionsData",
    }),
  },
  methods: {
    ...mapActions({
      setPositionsInsideOfTeam: "teams/setPositionsInsideOfTeam",
      setPositionsToLookup: "teams/setPositionsToLookup",
    }),
    routerPushToContact() {
      let generatePath = "/teams/" + this.$route.params.id + "/contact";
      this.$router.push({ path: generatePath });
    },
    customizeTooltip({ originalArgument, percent }) {
      return {
        text: `${this.positions[originalArgument].name} (${(
          percent * 100
        ).toFixed(2)}%)`,
      };
    },
    customizeLegend(e) {
      return this.positions[e.pointIndex].name;
    },
  },
  mounted() {
    this.setPositionsInsideOfTeam(this.$route.params.id);

    this.setPositionsToLookup().then((response) => {
      this.positions = response.data;
    });
  },
  components: {
    DxButton,
    PlayersList,
    DxPieChart,
    DxLegend,
    DxSeries,
    DxTooltip,
    DxLabel,
    DxConnector,
    DxSize,
    MatchesList,
  },
};
</script>
