<template>
  <div class="big-data-grid" v-if="getPlayerDetails">
    <h3>
      Witaj, {{ getPlayerDetails.FirstName }} {{ getPlayerDetails.LastName }}!
    </h3>
    <p>Twoja drużyna:</p>
    <PlayersList :isProfileView="true" :teamId="getPlayerDetails.TeamId" />
  </div>
</template>
<script>
import { mapGetters, mapActions } from "vuex";
import PlayersList from "../players/PlayersList";

export default {
  name: "UserProfile",
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
      getPlayerDetails: "players/getPlayerDetails",
    }),
  },
  methods: {
    ...mapActions({
      getPlayerDetailsByUserId: "players/getPlayerDetailsByUserId",
    }),
  },
  mounted() {
    if (this.getLoggedInUser) {
      this.getPlayerDetailsByUserId(this.getLoggedInUser.id);
    }
  },
  components: {
    PlayersList,
  },
};
</script>
