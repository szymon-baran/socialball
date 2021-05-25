<template>
  <div class="big-data-grid">
    <h3>Witaj, {{ getUserFullname }}!</h3>
    <p>Twoja drużyna:</p>
    <PlayersList 
      :isProfileView="true"
      :profileTeamId="getCurrentUser.teamId"
    />
  </div>
</template>
<script>
import { mapGetters } from "vuex";
import PlayersList from "../players/PlayersList";

export default {
  name: "UserProfile",
  computed: {
    ...mapGetters({
      getUserFullname: "getUserFullname",
      getCurrentUser: "getCurrentUser",
    }),
  },
  components: {
    PlayersList
  },
};
</script>
