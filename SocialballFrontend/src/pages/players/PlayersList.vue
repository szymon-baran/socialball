<template>
  <div v-bind:class="{ 'big-data-grid': !isProfileView }">
    <h3 v-if="!isProfileView">Lista wszystkich zawodników</h3>
    <DxDataGrid
      :data-source="getPlayers"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
      :hover-state-enabled="true"
      @row-click="onRowClick"
      :column-auto-width="true"
      width="100%"
      no-data-text="Brak zawodników"
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
        v-if="teamId"
        data-field="isInjured"
        caption="Czy kontuzjowany"
        data-type="boolean"
      />
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
  name: "Players",
  data() {
    return {
      positions: [],
    };
  },
  props: {
    isProfileView: {
      type: Boolean,
      required: false,
    },
    teamId: {
      type: String,
      required: false,
    },
  },
  computed: {
    ...mapGetters({
      getPlayers: "players/getPlayers",
      getTeams: "teams/getTeams",
    }),
  },
  methods: {
    ...mapActions({
      setPlayers: "players/setPlayers",
      setAllTeams: "teams/setAllTeams",
      setPositionsToLookup: "teams/setPositionsToLookup",
    }),
    ...mapMutations({
      RESET_PLAYERS: "players/RESET_PLAYERS",
    }),
    onRowClick(e) {
      this.$router.push({ path: `/players/${e.data.id}` });
    },
  },
  mounted() {
    this.setPositionsToLookup().then((response) => {
      this.positions = response.data;
    });
    this.setPlayers(this.teamId);
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
    this.RESET_PLAYERS();
  },
};
</script>
