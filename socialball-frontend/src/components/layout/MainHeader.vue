<template>
  <header>
    <nav>
      <h1>
        <router-link to="/"
          ><i class="fas fa-futbol"></i>

          SOCIALBALL</router-link
        >
      </h1>
      <ul>
        <li>
          <router-link to="/teams">Drużyny</router-link>
        </li>
        <li>
          <router-link to="/players">Piłkarze</router-link>
        </li>
        <li>
          <router-link to="/matches">Mecze</router-link>
        </li>
        <li>
          <router-link to="/messages">Wiadomości</router-link>
        </li>
        <!-- TO DO -->
      </ul>
      <div>
        <ul>
          <li>
            <h3>
              <router-link to="/profile">
                Manchester United
              </router-link>
            </h3>
          </li>
          <li>
            <!-- <router-link to="/login">Logowanie</router-link> -->
            <router-link to="/logout">Wyloguj</router-link>
          </li>
        </ul>
      </div>
    </nav>
  </header>
</template>

<style scoped>
header {
  width: 100%;
  height: 6vh;
  background-color: #011b01;
  display: flex;
  justify-content: center;
  align-items: center;
}

header a {
  text-decoration: none;
  color: whitesmoke;
  display: inline-block;
  padding: 0.6rem 1.2rem;
  border: 1px solid transparent;
  border-radius: 8px;
}

a:active,
a.router-link-active {
  border: 1px solid whitesmoke;
  border-radius: 8px;
  transform: scale(1.1, 1.1);
  background-color: whitesmoke;
  color: black;
}

a:hover {
  border: 1px solid whitesmoke;
  border-radius: 8px;
}

h3, h1 {
  margin: 0;
}

h1 a:hover,
h1 a:active,
h1 a.router-link-active {
  border: none;
  background-color: inherit;
  color: white;
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
}
</style>
