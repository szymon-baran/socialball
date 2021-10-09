<template>
  <div class="big-data-grid">
    <h3>Szczegóły zawodnika</h3>
    <player-registration 
      :showAsDetails="true"
    />
    <div class="mt-4">
      <DxButton text="Kontakt" @click="routerPushToContact" />
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import DxButton from "devextreme-vue/button";
import PlayerRegistration from './PlayerRegistration.vue';

export default {
  name: "PlayerDetails",
  computed: {
    ...mapGetters({
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
    DxButton,
    PlayerRegistration,
  },
};
</script>
