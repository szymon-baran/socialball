<template>
  <div class="big-data-grid">
    <h3>
      Witaj, {{ getPlayerDetails.FirstName }} {{ getPlayerDetails.LastName }}!
    </h3>
    <div v-if="userTeamId">
      <h4>
        Drużyna:
        <router-link
          :to="{ name: 'teamDetails', params: { id: userTeamId } }"
          >{{ getPlayerDetails.Team.name }}</router-link
        >
      </h4>
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
      <h4 class="line">Co chcesz dzisiaj zrobić?</h4>
      <div class="row mb-4 text-center">
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Edytuj profil"
            type="default"
            width="300px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Baza zawodników"
            type="default"
            @click="routerPushToPlayers"
            width="300px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Baza drużyn"
            type="default"
            @click="routerPushToTeams"
            width="300px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Baza meczy"
            type="default"
            @click="routerPushToMatches"
            width="300px"
            height="150px"
          />
        </div>
      </div>
    </div>
    <div>
      <h4 class="line">Funkcje zarządu drużyny</h4>
      <div class="row text-center">
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Edycja drużyny"
            type="danger"
            width="300px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Ogłoszenia zawodników"
            type="danger"
            @click="routerPushToJobAdvertisements"
            width="300px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Niepotwierdzone mecze"
            type="danger"
            @click="routerPushToUnconfirmedMatches"
            width="300px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Transfery drużyny"
            type="danger"
            width="300px"
            height="150px"
          />
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { mapGetters, mapActions } from "vuex";
import { userTypeEnum } from "../../enums/userTypeEnum";
import DxButton from "devextreme-vue/button";

export default {
  name: "UserProfile",
  data() {
    return {
      userTypeEnum,
      userTeamId: null,
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
      getUserTeamId: "authentication/getUserTeamId",
    }),
    routerPushToPlayers() {
      this.$router.push({ path: `/players` });
    },
    routerPushToTeams() {
      this.$router.push({ path: `/teams` });
    },
    routerPushToMatches() {
      this.$router.push({ path: `/matches` });
    },
    routerPushToJobAdvertisements() {
      this.$router.push({ path: `/job-advertisements` });
    },
    routerPushToUnconfirmedMatches() {
      this.$router.push({ path: `/unconfirmed-matches` });
    },
  },
  mounted() {
    this.getUserTeamId().then((response) => {
      this.userTeamId = response.data;
    });
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
  },
};
</script>
