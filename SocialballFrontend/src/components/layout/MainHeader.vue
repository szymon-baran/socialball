<template>
  <header>
    <nav>
      <h2>
        <router-link to="/" draggable="false"
          ><i class="fas fa-futbol"></i>

          SOCIALBALL</router-link
        >
      </h2>
      <ul>
        <li>
          <router-link
            :class="{ 'router-link-active': checkForTeamsPath }"
            to="/teams"
            draggable="false"
            >Drużyny</router-link
          >
        </li>
        <li>
          <router-link
            :class="{ 'router-link-active': checkForPlayersPath }"
            to="/players"
            draggable="false"
            >Piłkarze</router-link
          >
        </li>
        <li>
          <router-link to="/matches" draggable="false">Mecze</router-link>
        </li>
        <li>
          <router-link to="/messages" draggable="false">Wiadomości</router-link>
        </li>
        <!-- TO DO -->
      </ul>
      <div>
        <ul>
          <li class="font-weight-bold">
            <router-link to="/profile" draggable="false">
              <i class="far fa-user-circle"></i>
              <span class="ml-2">{{ getUserFullname }}</span>
            </router-link>
          </li>
          <li>
            <!-- <router-link to="/login">Logowanie</router-link> -->
            <!-- <router-link to="/logout">Wyloguj</router-link> -->
            <i class="fas fa-sign-out-alt"></i>
            <span class="ml-2">Wyloguj</span>
          </li>
        </ul>
      </div>
    </nav>
  </header>
</template>
<script>
import { mapGetters } from "vuex";

export default {
  name: "MainHeader",
  computed: {
    ...mapGetters({ getUserFullname: "getUserFullname" }),
    checkForTeamsPath() {
      return this.$route.fullPath.includes("/teams") ? true : false;
    },
    checkForPlayersPath() {
      return this.$route.fullPath.includes("/players") ? true : false;
    },
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
}

ul {
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
</style>
