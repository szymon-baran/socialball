<template>
  <div class="big-data-grid">
    <h3>Rejestracja zawodnika</h3>
    <form @submit="handleSubmit">
      <DxForm :form-data="player">
        <DxGroupItem :col-count="3" caption="Dane o zawodniku">
          <DxSimpleItem
            data-field="FirstName"
            :is-required="true"
            :label="{ text: 'Imię' }"
          />
          <DxSimpleItem
            data-field="LastName"
            :is-required="true"
            :label="{ text: 'Nazwisko' }"
          />
          <DxSimpleItem data-field="Email">
            <DxEmailRule />
          </DxSimpleItem>
          <DxSimpleItem
            data-field="Position"
            editor-type="dxSelectBox"
            :editor-options="positionOptions"
            :label="{ text: 'Pozycja' }"
          />
          <DxSimpleItem
            data-field="TeamId"
            editor-type="dxSelectBox"
            :editor-options="{
              dataSource: this.getTeams,
              valueExpr: 'id',
              displayExpr: 'name',
            }"
            :label="{ text: 'Drużyna' }"
          />
          <DxSimpleItem
            data-field="Citizenship"
            :label="{ text: 'Narodowość' }"
          />
        </DxGroupItem>
        <DxGroupItem :col-count="2" caption="Dane do logowania zawodnika">
          <DxSimpleItem
            data-field="LoginUsername"
            :label="{ text: 'Nazwa użytkownika' }"
          />
          <DxSimpleItem
            data-field="LoginPassword"
            :label="{ text: 'Hasło' }"
            editor-type="dxTextBox"
            :editor-options="{
              mode: 'password'
            }"
          />
        </DxGroupItem>
        <DxButtonItem :button-options="submitButtonOptions" />
      </DxForm>
    </form>
  </div>
</template>
<script>
import {
  DxForm,
  DxSimpleItem,
  DxGroupItem,
  DxButtonItem,
  DxEmailRule,
} from "devextreme-vue/form";

import { mapActions, mapGetters } from "vuex";
import { useToast } from "vue-toastification";
import DataSource from "devextreme/data/data_source";

const submitButtonOptions = {
  text: "Zarejestruj zawodnika",
  useSubmitBehavior: true,
};

export default {
  components: {
    DxForm,
    DxSimpleItem,
    DxGroupItem,
    DxButtonItem,
    DxEmailRule,
  },
  data() {
    const positions = [
      { Id: 0, Name: "Bramkarz" },
      { Id: 1, Name: "Obrońca" },
      { Id: 2, Name: "Pomocnik" },
      { Id: 3, Name: "Napastnik" },
    ];
    return {
      player: {
        FirstName: "",
        LastName: "",
        Team: "",
        Email: "",
        Position: null,
      },
      submitButtonOptions,
      positionOptions: {
        dataSource: new DataSource({
          store: positions,
        }),
        valueExpr: "Id",
        displayExpr: "Name",
      },
    };
  },
  computed: {
    ...mapGetters({ getTeams: "teams/getTeams" }),
  },
  methods: {
    ...mapActions({
      addPlayer: "players/addPlayer",
      setAllTeams: "teams/setAllTeams",
    }),

    async handleSubmit(e) {
      e.preventDefault();
      await this.addPlayer(this.player);
      useToast().success("Zawodnik dodany pomyślnie!");
      this.$router.push({ path: "/players" });
    },
  },
  mounted() {
    this.setAllTeams();
  },
};
</script>
