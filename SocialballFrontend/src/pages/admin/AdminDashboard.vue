<template>
  <div class="big-data-grid">
    <div v-if="getLoggedInUser" class="text-center mb-3">
      <h3>
        Panel administratora
      </h3>
      <h4 class="red-color">
        Korzystanie z panelu zabronione dla osób nieupoważnionych!
      </h4>
    </div>
    <div class="primary-border">
      <h4 class="line">Baza danych strony</h4>
      <div class="row mb-4 text-center">
        <div class="col-6 col-md-4 mb-3">
          <DxButton
            text="Baza zawodników"
            type="default"
            @click="routerPushToPlayers"
            width="350px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-4 mb-3">
          <DxButton
            text="Tabele ligowe"
            type="default"
            @click="routerPushToTeams"
            width="350px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-4 mb-3">
          <DxButton
            text="Baza meczy"
            type="default"
            @click="routerPushToMatches"
            width="350px"
            height="150px"
          />
        </div>
      </div>
    </div>
    <div class="secondary-border">
      <h4 class="line">Funkcje administracyjne</h4>
      <div class="row mb-4 text-center">
        <div class="col-6 col-md-6 mb-3">
          <DxButton
            text="Zarządzanie użytkownikami"
            @click="routerPushToUsersAdmin"
            type="danger"
            width="500px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-6 mb-3">
          <DxButton
            text="Zarządzanie drużynami"
            @click="routerPushToTeamsAdmin"
            type="danger"
            width="500px"
            height="150px"
          />
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { mapGetters, mapActions, mapMutations } from "vuex";
import { userTypeEnum } from "../../enums/userTypeEnum";
import DxButton from "devextreme-vue/button";

export default {
  name: "AdminDashboard",
  data() {
    return {
      userTypeEnum,
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
    }),
  },
  methods: {
    ...mapActions({
      getUserDataByUserId: "players/getUserDataByUserId",
    }),
    ...mapMutations({
      RESET_PLAYER_FORM: "players/RESET_PLAYER_FORM",
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
    routerPushToUsersAdmin() {
      this.$router.push({ path: `/admin-users` });
    },
    routerPushToTeamsAdmin() {
      this.$router.push({ path: `/admin-teams` });
    },
  },
  mounted() {},
  components: {
    DxButton,
  },
};
</script>
