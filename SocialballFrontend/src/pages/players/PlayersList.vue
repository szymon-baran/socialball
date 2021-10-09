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
    >
      <DxFilterRow :visible="true" />
      <DxColumn data-field="firstName" caption="Imię" />
      <DxColumn data-field="lastName" caption="Nazwisko" />
      <DxColumn data-field="position" caption="Pozycja">
        <DxLookup
          :data-source="positions"
          value-expr="value"
          display-expr="name"
        />
      </DxColumn>
      <DxColumn data-field="teamId" caption="Klub">
        <DxLookup :data-source="getTeams" value-expr="id" display-expr="name" />
      </DxColumn>
      <DxColumn data-field="citizenship" caption="Narodowość" />
    </DxDataGrid>
    <p v-if="!isProfileView" class="mt-3">
      Na liście nie ma zawodnika? Zarejestruj go
      <router-link to="/register">tutaj</router-link>!
    </p>
  </div>
</template>

<script>
import {
  DxDataGrid,
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
    DxColumn,
    DxFilterRow,
    DxLookup,
  },
  beforeUnmount() {
    this.RESET_PLAYERS();
  },
};
</script>
