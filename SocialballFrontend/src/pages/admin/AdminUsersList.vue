<template>
  <div class="big-data-grid">
    <h3>Zarządzanie użytkownikami (Administrator)</h3>
    <DxDataGrid
      :data-source="getPlayers"
      :remote-operations="false"
      :row-alternation-enabled="true"
      :show-borders="true"
      :hover-state-enabled="true"
      :column-auto-width="true"
      width="100%"
      no-data-text="Brak użytkowników"
    >
      <DxFilterRow :visible="true" />
      <DxLoadPanel :enabled="true" />
      <DxPaging :page-size="15" />
      <DxColumn
        data-field="username"
        caption="Nazwa użytkownika"
        data-type="string"
      />
      <DxColumn
        data-field="name"
        caption="Nazwisko i imię"
        data-type="string"
      />
      <DxColumn data-field="userType" caption="Typ użytkownika">
        <DxLookup
          :data-source="userTypes"
          value-expr="value"
          display-expr="name"
        />
      </DxColumn>
      <DxColumn
        data-field="isActive"
        caption="Czy aktywny?"
        data-type="boolean"
      />
      <DxColumn
        data-field="id"
        caption="Akcje"
        cell-template="actions-template"
        :allow-sorting="false"
        :allow-filtering="false"
        :allow-search="false"
        alignment="center"
      />
      <template #actions-template="{ data }">
        <div>
          <span
            class="action"
            @click="onUserBan({ data })"
            title="Zablokuj użytkownika"
            v-if="data.data.isActive"
            ><i class="fas fa-lock"></i
          ></span>
          <span
            class="action"
            @click="onUserUnban({ data })"
            title="Odblokuj użytkownika"
            v-else
            ><i class="fas fa-unlock"></i
          ></span>
          <a
            :href="getUserMail({ data })"
            class="action"
            title="Napisz wiadomość mailową"
            v-if="data.data.email"
            ><i class="fas fa-envelope ml-2"></i
          ></a>
          <a
            class="action"
            title="Usuń zdjęcie użytkownika"
            @click="handlePhotoDelete({ data })"
            ><i class="fas fa-image ml-2"></i
          ></a>
        </div>
      </template>
    </DxDataGrid>
  </div>
</template>

<script>
import {
  DxDataGrid,
  DxLoadPanel,
  DxColumn,
  DxFilterRow,
  DxLookup,
  DxPaging,
} from "devextreme-vue/data-grid";

import { mapGetters, mapActions, mapMutations } from "vuex";
import { useToast } from "vue-toastification";
import { custom } from "devextreme/ui/dialog";

export default {
  name: "AdminUsersList",
  data() {
    return {
      userTypes: [],
    };
  },
  computed: {
    ...mapGetters({
      getPlayers: "players/getPlayers",
    }),
  },
  methods: {
    ...mapActions({
      setUsersAdmin: "players/setUsersAdmin",
      setUserTypesToLookup: "players/setUserTypesToLookup",
      banUserAdmin: "players/banUserAdmin",
      unbanUserAdmin: "players/unbanUserAdmin",
      userImageDeleteAdmin: "players/userImageDeleteAdmin",
    }),
    ...mapMutations({
      RESET_PLAYERS: "players/RESET_PLAYERS",
    }),
    onUserBan(data) {
      this.banUserAdmin(data.data.value);
      useToast().success("Użytkownik zablokowany pomyślnie!");
    },
    onUserUnban(data) {
      this.unbanUserAdmin(data.data.value);
      useToast().success("Użytkownik odblokowany pomyślnie!");
    },
    getUserMail(data) {
      return data.data.data.email ? "mailto:" + data.data.data.email : "";
    },
    handlePhotoDelete(data) {
      let dialog = custom({
        title: "Potwierdzenie",
        messageHtml: `Czy na pewno chcesz usunąć zdjęcie profilowe użytkownika ${data.data.data.name}?`,
        buttons: [
          {
            text: "Tak",
            onClick: () => {
              return true;
            },
          },
          {
            text: "Nie",
            onClick: () => {
              return false;
            },
          },
        ],
      });
      dialog.show().then((dialogResult) => {
        if (dialogResult === true) {
          this.userImageDeleteAdmin(data.data.data.userDataId);
          this.popupVisible = false;
          this.$emit("closed");
          useToast().success("Zdjęcie użytkownika zostało usunięte!");
        }
      });
    },
  },
  mounted() {
    this.setUserTypesToLookup().then((response) => {
      this.userTypes = response.data;
    });
    this.setUsersAdmin();
  },
  components: {
    DxDataGrid,
    DxLoadPanel,
    DxColumn,
    DxFilterRow,
    DxLookup,
    DxPaging,
  },
  beforeUnmount() {
    this.RESET_PLAYERS();
  },
};
</script>
<style scoped>
.action {
  font-size: 1.1em;
}
.action:hover {
  color: #009385;
}
</style>
