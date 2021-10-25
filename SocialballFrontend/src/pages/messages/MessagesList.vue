<template>
  <div class="big-data-grid">
    <h3>Wiadomości</h3>
    <TeamMessagesList />
  </div>
</template>
<script>
import TeamMessagesList from "./TeamMessagesList";

export default {
  name: "MessagesList",
  components: {
    TeamMessagesList,
  },
};
</script>
