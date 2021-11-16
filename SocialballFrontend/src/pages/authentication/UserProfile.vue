<template>
  <div class="big-data-grid">
    <h3>
      Witaj, {{ getPlayerDetails.FirstName }} {{ getPlayerDetails.LastName }}!
    </h3>
    <div v-if="getLoggedInUser.teamId">
      <h4 class="line">
        Twoja drużyna to {{ getPlayerDetails.Team.name }}, a jej aktualna kadra
        składa się z następujących zawodników:
      </h4>
      <PlayersList
        :isProfileView="true"
        :teamId="getPlayerDetails.TeamId"
        v-if="getPlayerDetails.TeamId"
      />
    </div>
    <div v-else>
      <h4>
        Aktualnie nie posiadasz drużyny. Ale nie przejmuj się! Nasza strona może
        pomóc Ci rozwiązać ten problem! Jeżeli jesteś zainteresowany, skorzystaj
        z przycisku poniżej.
      </h4>
      <DxButton
        text="Poszukiwanie drużyny"
        type="default"
        styling-mode="outlined"
        @click="routerPushToJobAdvertisements"
      />
    </div>
    <div>
      <h4 class="line">Akcje</h4>
    </div>
  </div>
</template>
<script>
import { mapGetters, mapActions } from "vuex";
import PlayersList from "../players/PlayersList";
import { userTypeEnum } from "../../enums/userTypeEnum";
import DxButton from "devextreme-vue/button";

export default {
  name: "UserProfile",
  data() {
    return {
      userTypeEnum,
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
      getPlayerDetails: "players/getPlayerDetails",
    }),
  },
  methods: {
    ...mapActions({
      getPlayerDetailsByUserId: "players/getPlayerDetailsByUserId",
      getUserDataByUserId: "players/getUserDataByUserId",
    }),
    routerPushToJobAdvertisements() {
      this.$router.push({ path: `/job-advertisements` });
    },
  },
  mounted() {
    if (this.getLoggedInUser) {
      switch (this.getLoggedInUser.userType) {
        case userTypeEnum.PLAYER:
          this.getPlayerDetailsByUserId(this.getLoggedInUser.id);
          break;
        default:
          this.getUserDataByUserId(this.getLoggedInUser.id);
          break;
      }
    }
  },
  components: {
    DxButton,
    PlayersList,
  },
};
</script>
