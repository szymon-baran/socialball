<template>
  <header>
    <nav>
      <h2>
        <router-link to="/" draggable="false"
          ><i class="fas fa-futbol"></i>

          socialball</router-link
        >
      </h2>
      <ul class="nav-menu" v-bind:class="[isMobileMenuOpened ? 'active' : '']">
        <li v-if="!isLoggedIn">
          <router-link
            :class="{ 'router-link-active': checkForTeamsPath }"
            to="/teams"
            draggable="false"
            >Tabele ligowe</router-link
          >
        </li>
        <li v-if="!isLoggedIn">
          <router-link
            :class="{ 'router-link-active': checkForPlayersPath }"
            to="/players"
            draggable="false"
            >Zawodnicy</router-link
          >
        </li>
        <li v-if="!isLoggedIn">
          <router-link to="/matches" draggable="false">Mecze</router-link>
        </li>
        <li v-if="isLoggedIn && getLoggedInUser.userType != 10">
          <router-link to="/messages" draggable="false"
            ><i class="fas fa-envelope"></i> <span class="ml-1">Wiadomości</span></router-link
          >
        </li>
        <li v-if="isLoggedIn && !userTeamId && getLoggedInUser.userType != 10">
          <router-link to="/job-advertisements" draggable="false"
            >Poszukiwanie drużyny</router-link
          >
        </li>
        <li v-if="isLoggedIn && userTeamId">
          <router-link
            :to="{ name: 'teamDetails', params: { id: userTeamId } }"
            draggable="false"
            ><i class="fas fa-users"></i> <span class="ml-1">Moja drużyna</span></router-link
          >
        </li>
        <!-- </ul>
      <div>
        <ul class="nav-menu" v-bind:class="[isMobileMenuOpened ? 'active' : '']"> -->
        <li class="font-weight-bold" v-if="isLoggedIn">
          <router-link to="/profile" draggable="false">
            <i class="fas fa-user-circle mr-2"></i>
            <span>{{ getUsername }}</span>
          </router-link>
          <a
            v-if="isLoggedIn"
            class="login-buttons"
            @click="logoutMethod"
            title="Wyloguj"
            ><i class="fas fa-sign-out-alt"></i
          ></a>
        </li>
        <li v-if="!isLoggedIn">
          <span
            ><a
              class="login-buttons"
              id="showLoginButton"
              @click="showLoginPopover"
              title="Zaloguj"
              ><i class="fas fa-sign-in-alt"></i> </a
          ></span>
        </li>
      </ul>
      <!-- </div> -->
      <div
        class="hamburger"
        @click="openMobileMenu"
        v-bind:class="[isMobileMenuOpened ? 'active' : '']"
      >
        <span class="bar"></span>
        <span class="bar"></span>
        <span class="bar"></span>
      </div>
    </nav>
  </header>
  <LoginPlayer
    :loginPopoverVisible="loginPopoverVisible"
    @hideLoginForm="onLoginFormHiding"
    v-if="loginPopoverVisible"
  />
</template>
<script>
import { mapActions, mapGetters } from "vuex";
import LoginPlayer from "../../pages/authentication/LoginPlayer";
import { useToast } from "vue-toastification";
import { userTypeEnum } from "../../enums/userTypeEnum";

export default {
  name: "MainHeader",
  data() {
    return {
      userTypeEnum,
      loginPopoverVisible: false,
      userTeamId: null,
      isMobileMenuOpened: false,
    };
  },
  computed: {
    ...mapGetters({
      getUsername: "authentication/getUsername",
      isLoggedIn: "authentication/isLoggedIn",
      getLoggedInUser: "authentication/getLoggedInUser",
    }),
    checkForTeamsPath() {
      return this.$route.fullPath.includes("/teams") ? true : false;
    },
    checkForPlayersPath() {
      return this.$route.fullPath.includes("/players") ? true : false;
    },
  },
  methods: {
    ...mapActions({
      logout: "authentication/logout",
      getUserTeamId: "authentication/getUserTeamId",
    }),
    showLoginPopover() {
      this.isMobileMenuOpened = false;
      this.loginPopoverVisible = true;
    },
    onLoginFormHiding() {
      this.loginPopoverVisible = false;
    },
    logoutMethod() {
      this.logout();
      this.$router.push({ path: "/" });
      useToast().success("Wylogowano pomyślnie!");
    },
    openMobileMenu() {
      this.isMobileMenuOpened = !this.isMobileMenuOpened;
    },
  },
  mounted() {
    if (this.isLoggedIn) {
      this.getUserTeamId().then((response) => {
        this.userTeamId = response.data;
      });
    }
  },
  components: {
    LoginPlayer,
  },
};
</script>
<style scoped>
header {
  font-family: "Montserrat", sans-serif;
  width: 100%;
  height: 6vh;
  background-color: #363640;
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 88888;
  border-bottom: 1px solid #898d90;
  border-radius: 10px;
}

header a {
  text-decoration: none;
  color: whitesmoke;
  display: inline-block;
  padding: 0.2rem 1.2rem;
  border: 1px solid transparent;
  border-radius: 8px;
}

a:active,
a.router-link-active {
  border-bottom: 2px solid #009385;
  border-radius: 2px;
  color: #009385;
}

a:hover {
  color: #009385;
}

h2 {
  font-size: 150%;
  margin: 0;
  font-weight: bold;
  letter-spacing: 0.06em;
}

h2 a:hover,
h2 a:active,
h2 a.router-link-active {
  border-bottom: none;
  color: #009385;
  transition: all 0.4s ease;
  transform: scale(1.07);
}

nav {
  width: 90%;
  margin: auto;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-radius: 10px;
}

.nav-menu {
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  justify-content: center;
  align-items: center;
}

li {
  margin: 0 0.6rem;
  font-size: 110%;
}

router-link {
  pointer-events: none;
}

.login-buttons {
  cursor: pointer;
}

.hamburger {
  display: none;
}

.bar {
  display: block;
  width: 25px;
  height: 3px;
  margin: 5px auto;
  -webkit-transition: all 0.3s ease-in-out;
  transition: all 0.3s ease-in-out;
  background-color: whitesmoke;
}

@media only screen and (max-width: 940px) {
  .nav-menu {
    position: fixed;
    left: -100%;
    top: 6vh;
    flex-direction: column;
    background-color: #363640;
    width: 100%;
    border-radius: 10px;
    text-align: center;
    transition: 0.5s;
    box-shadow: 0 10px 27px rgba(0, 0, 0, 0.05);
    z-index: 88888;
  }

  .nav-menu.active {
    left: 0;
  }

  li {
    margin: 2.5vh 0;
  }

  .hamburger {
    display: block;
    cursor: pointer;
  }

  .hamburger.active .bar:nth-child(2) {
    opacity: 0;
  }

  .hamburger.active .bar:nth-child(1) {
    transform: translateY(8px) rotate(45deg);
  }

  .hamburger.active .bar:nth-child(3) {
    transform: translateY(-8px) rotate(-45deg);
  }
}
</style>
