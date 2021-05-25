<template>
  <div class="big-data-grid">
    <h3>Szczegóły zawodnika</h3>
    <h4>Imię</h4>
    <DxTextBox
      v-model="getPlayerDetails.firstName"
      :disabled="true"
      :width="150"
    />
    <h4>Nazwisko</h4>
    <DxTextBox
      v-model="getPlayerDetails.lastName"
      :disabled="true"
      :width="150"
    />
    <h4>Narodowość</h4>
    <DxTextBox
      v-model="getPlayerDetails.citizenship"
      :disabled="true"
      :width="150"
    />
    <h4>Nazwa użytkownika</h4>
    <DxTextBox
      v-model="getPlayerDetails.loginUsername"
      :disabled="true"
      :width="150"
    />
    <div class="mt-4">
      <DxButton text="Kontakt" @click="routerPushToContact" />
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import { DxTextBox } from "devextreme-vue/text-box";
import DxButton from "devextreme-vue/button";

export default {
  name: "PlayerDetails",
  computed: {
    ...mapGetters({ getPlayerDetails: "players/getPlayerDetails" }),
  },
  methods: {
    ...mapActions({
      setPlayerDetails: "players/setPlayerDetails",
    }),
    routerPushToContact() {
        let generatePath = "/players/" + this.$route.params.id + "/contact"
      this.$router.push({ path: generatePath });
    },
  },
  mounted() {
    this.setPlayerDetails(this.$route.params.id);
  },
  components: {
    DxTextBox,
    DxButton,
  },
};
</script>
