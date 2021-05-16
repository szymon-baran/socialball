<template>
  <div class="big-data-grid">
    <h3>Lista wszystkich zawodników</h3>
    <DxDataGrid
      caption="Lista wszystkich zawodników"
      :data-source="getPlayers"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
    >
      <DxFilterRow :visible="true" />
      <DxColumn data-field="firstName" caption="Imię" />
      <DxColumn data-field="lastName" caption="Nazwisko" />
      <DxColumn data-field="position" caption="Pozycja">
        <DxLookup
          :data-source="positionsToLookup"
          value-expr="id"
          display-expr="value"
        />
      </DxColumn>
      <DxColumn data-field="teamName" caption="Klub" />
      <DxColumn data-field="citizenship" caption="Narodowość" />
    </DxDataGrid>
    <p>
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

import { mapGetters, mapActions } from "vuex";

export default {
  name: "Players",
  data() {
    return {
      positionsToLookup: {
        store: {
          type: "array",
          data: [
            { id: 0, value: "Bramkarz" },
            { id: 1, value: "Obronca" },
            { id: 2, value: "Pomocnik" },
            { id: 3, value: "Napastnik" },
          ],
          key: "id",
        },
      },
    };
  },
  computed: {
    ...mapGetters({ getPlayers: "players/getPlayers" }),
  },
  methods: {
    ...mapActions({
      setPlayers: "players/getAllPlayers",
    }),
  },
  mounted() {
    this.setPlayers();
  },
  components: {
    DxDataGrid,
    DxColumn,
    DxFilterRow,
    DxLookup,
  },
};
</script>
