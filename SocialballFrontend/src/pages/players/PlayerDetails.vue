<template>
  <div class="big-data-grid">
    <h3>Szczegóły zawodnika</h3>
    <div class="row">
      <div class="col-3">
        <label>Imię</label>
        <DxTextBox
          v-model="getPlayerDetails.firstName"
          :disabled="true"
          :width="300"
        />
      </div>
      <div class="col-3">
        <label>Nazwisko</label>
        <DxTextBox
          v-model="getPlayerDetails.lastName"
          :disabled="true"
          :width="300"
        />
      </div>
    </div>
    <div class="row">
      <div class="col-3">
        <label class="mt-3">Klub</label>
        <DxSelectBox
          :data-source="getTeams"
          v-model="getPlayerDetails.teamId"
          :disabled="true"
          :width="300"
          value-expr="id"
          display-expr="name"
        />
      </div>
      <div class="col-3">
        <label class="mt-3">Narodowość</label>
        <DxTextBox
          v-model="getPlayerDetails.citizenship"
          :disabled="true"
          :width="300"
        />
      </div>
    </div>
    <div class="row">
      <div class="col-3">
        <label class="mt-3">Nazwa użytkownika</label>
        <DxTextBox
          v-model="getPlayerDetails.loginUsername"
          :disabled="true"
          :width="300"
        />
      </div>
      <div class="col-3">
        <label class="mt-3">E-mail</label>
        <DxTextBox
          v-model="getPlayerDetails.email"
          :disabled="true"
          :width="300"
        />
      </div>
    </div>
    <div class="mt-4">
      <DxButton text="Kontakt" @click="routerPushToContact" />
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import { DxTextBox } from "devextreme-vue/text-box";
import DxButton from "devextreme-vue/button";
import DxSelectBox from "devextreme-vue/select-box";

export default {
  name: "PlayerDetails",
  computed: {
    ...mapGetters({
      getPlayerDetails: "players/getPlayerDetails",
      getTeams: "teams/getTeams",
    }),
  },
  methods: {
    ...mapActions({
      setPlayerDetails: "players/setPlayerDetails",
      setAllTeams: "teams/setAllTeams",
    }),
    routerPushToContact() {
      let generatePath = "/players/" + this.$route.params.id + "/contact";
      this.$router.push({ path: generatePath });
    },
  },
  mounted() {
    this.setPlayerDetails(this.$route.params.id);
    this.setAllTeams();
  },
  components: {
    DxTextBox,
    DxButton,
    DxSelectBox,
  },
};
</script>
