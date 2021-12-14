<template>
  <!-- TO DO: wyrzucic druzyne: moze jakies zaproszenia wysylane przez zarzad + prosby o dodanie ze strony zawodnikow -->
  <div class="big-data-grid">
    <h3>{{ getTitle() }}</h3>
    <form>
      <DxValidationGroup :ref="groupRefKey">
        <div class="primary-border">
          <h4 class="line">Dane personalne</h4>
          <div class="row">
            <div class="col">
              <label for="firstNameTextBox" class="form-label">Imię</label>
              <DxTextBox v-model="FirstName" id="firstNameTextBox" />
            </div>
            <div class="col">
              <label for="lastNameTextBox" class="form-label">Nazwisko</label>
              <DxTextBox v-model="LastName" id="lastNameTextBox">
                <DxValidator>
                  <DxRequiredRule message="Nazwisko jest wymagane!" />
                </DxValidator>
              </DxTextBox>
            </div>
            <div class="col">
              <label for="dateOfBirthDateBox" class="form-label"
                >Data urodzenia</label
              >
              <DxDateBox
                v-model="DateOfBirth"
                id="dateOfBirthDateBox"
                type="date"
                display-format="dd/MM/yyyy"
                cancel-button-text="Anuluj"
                invalid-date-message="Wartość musi być datą lub czasem"
              />
            </div>
          </div>
          <div class="row mt-4">
            <div
              class="col"
              v-if="!this.showToEdit || this.getLoggedInUser.userType === 1"
            >
              <label for="positionSelectBox" class="form-label">Pozycja</label>
              <DxSelectBox
                :dataSource="positionsStore"
                value-expr="Id"
                display-expr="Name"
                v-model="Position"
                id="positionSelectBox"
              >
                <DxValidator>
                  <DxRequiredRule
                    message="Wybór preferowanej pozycji jest wymagany!"
                  />
                </DxValidator>
              </DxSelectBox>
            </div>
            <!-- <div class="col">
              <label for="teamSelectBox" class="form-label">Drużyna</label>
              <DxSelectBox
                :dataSource="getTeams"
                value-expr="id"
                display-expr="name"
                v-model="TeamId"
                id="teamSelectBox"
                :show-clear-button="true"
                :read-only="showToEdit"
              />
            </div> -->
            <div class="col">
              <label for="citizenshipTextBox" class="form-label"
                >Narodowość</label
              >
              <DxTextBox v-model="Citizenship" id="citizenshipTextBox" />
            </div>
          </div>
          <div class="row mt-4">
            <div class="col">
              <label for="imageUploader" class="form-label"
                >Zdjęcie profilowe</label
              >
              <DxFileUploader
                select-button-text="Dodaj zdjęcie"
                label-text="zaleca się obraz kwadratowy"
                accept="image/*"
                upload-mode="useForm"
                id="imageUploader"
                readyToUploadMessage="Gotowe do przesłania"
                @value-changed="imageAdded"
              />
            </div>
          </div>
        </div>
        <div>
          <div class="secondary-border">
            <h4 class="line">Dane techniczne</h4>
            <div class="row">
              <div class="col" v-if="!showToEdit">
                <label for="loginUsernameTextBox" class="form-label"
                  >Nazwa użytkownika</label
                >
                <DxTextBox v-model="LoginUsername" id="loginUsernameTextBox">
                  <DxValidator>
                    <DxRequiredRule
                      message="Nazwa użytkownika jest wymagana!"
                    />
                    <DxAsyncRule
                      :validation-callback="validateUsername"
                      message="Taki użytkownik istnieje już w systemie!"
                    />
                  </DxValidator>
                </DxTextBox>
              </div>
              <div class="col">
                <label for="emailTextBox" class="form-label">E-mail</label>
                <DxTextBox v-model="Email" id="emailTextBox">
                  <DxValidator>
                    <DxEmailRule message="E-mail jest nieprawidłowy." />
                  </DxValidator>
                </DxTextBox>
              </div>
            </div>
            <div class="row mt-4" v-if="!showToEdit">
              <div class="col">
                <label for="loginPasswordTextBox" class="form-label"
                  >Hasło</label
                >
                <DxTextBox
                  v-model="LoginPassword"
                  id="loginPasswordTextBox"
                  mode="password"
                >
                  <DxValidator>
                    <DxRequiredRule message="Hasło jest wymagane!" />
                  </DxValidator>
                </DxTextBox>
              </div>
              <div class="col">
                <label for="loginPasswordConfirmationTextBox" class="form-label"
                  >Powtórz hasło</label
                >
                <DxTextBox
                  id="loginPasswordConfirmationTextBox"
                  mode="password"
                >
                  <DxValidator>
                    <DxRequiredRule
                      message="Powtórzenie hasła jest wymagane!"
                    />
                    <DxCompareRule
                      :comparison-target="passwordComparison"
                      message="Hasła się nie zgadzają!"
                    />
                  </DxValidator>
                </DxTextBox>
              </div>
            </div>
            <div class="row mt-4" v-if="!this.$route.params.playerToEditId">
              <div class="px-3">
                <vue-recaptcha
                  v-show="true"
                  siteKey="6LejOUwdAAAAAJYhE8VtA-J0PNrriXDlNyLg9ETw"
                  size="normal"
                  theme="dark"
                  hl="pl"
                  @verify="recaptchaVerified"
                  @expire="recaptchaExpired"
                  @fail="recaptchaFailed"
                  ref="vueRecaptcha"
                  :style="{ border: borderStyle }"
                >
                </vue-recaptcha>
                <DxValidator :adapter="recaptchaValidatorConfig">
                  <DxRequiredRule
                    message="Zaznacz pole 'Nie jestem robotem'."
                  />
                </DxValidator>
              </div>
            </div>
          </div>
          <div class="row mt-4">
            <div class="col">
              <DxValidationSummary />
            </div>
            <div class="col text-right">
              <DxButton
                :text="getSubmitText()"
                type="default"
                @click="handleSubmit"
                ref="submitButton"
              />
            </div>
          </div>
        </div>
      </DxValidationGroup>
    </form>
  </div>
</template>
<script>
import DxTextBox from "devextreme-vue/text-box";
import DxSelectBox from "devextreme-vue/select-box";
import DxButton from "devextreme-vue/button";
import {
  DxValidator,
  DxRequiredRule,
  DxEmailRule,
  DxCompareRule,
  DxAsyncRule,
} from "devextreme-vue/validator";
import DxValidationGroup from "devextreme-vue/validation-group";
import DxValidationSummary from "devextreme-vue/validation-summary";

import { mapActions, mapGetters, mapMutations } from "vuex";
import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "players/getField",
  mutationType: "players/updateField",
});

import { useToast } from "vue-toastification";
import DataSource from "devextreme/data/data_source";
import vueRecaptcha from "vue3-recaptcha2";
import DxDateBox from "devextreme-vue/date-box";
import { DxFileUploader } from "devextreme-vue/file-uploader";
import { userTypeEnum } from "../../enums/userTypeEnum";

export default {
  data() {
    const positions = [
      { Id: 0, Name: "Bramkarz" },
      { Id: 1, Name: "Obrońca" },
      { Id: 2, Name: "Pomocnik" },
      { Id: 3, Name: "Napastnik" },
    ];
    const callbacks = [];
    const recaptchaValidatorConfig = {
      getValue: () => {
        return this.isRecaptchaVerified;
      },
      applyValidationResults: (e) => {
        this.borderStyle = e.isValid ? "none" : ".5px solid #813e3c";
      },
      validationRequestsCallbacks: callbacks,
    };
    return {
      showToEdit: false,
      player: {},
      positionsStore: new DataSource({ store: positions }),
      groupRefKey: "targetGroup",
      isRecaptchaVerified: false,
      recaptchaValidatorConfig,
      borderStyle: "none",
      userTypeEnum,
    };
  },
  computed: {
    ...mapGetters({
      getTeams: "teams/getTeams",
      getLoggedInUser: "authentication/getLoggedInUser",
    }),
    ...mapFields([
      "player.Id",
      "player.FirstName",
      "player.LastName",
      "player.DateOfBirth",
      "player.Email",
      "player.Position",
      "player.TeamId",
      "player.Citizenship",
      "player.LoginUsername",
      "player.LoginPassword",
      "player.Image",
    ]),
    validationGroup: function() {
      return this.$refs[this.groupRefKey].instance;
    },
  },
  methods: {
    ...mapActions({
      addPlayer: "players/addPlayer",
      editPlayer: "players/editPlayer",
      validateUsername: "players/validateUsername",
      setAllTeams: "teams/setAllTeams",
      setPlayerDetails: "players/setPlayerDetails",
      setUserDataDetails: "players/setUserDataDetails",
    }),
    ...mapMutations({
      RESET_PLAYER_FORM: "players/RESET_PLAYER_FORM",
    }),
    getTitle() {
      if (this.showToEdit) {
        return "Edycja profilu";
      } else {
        return "Rejestracja zawodnika";
      }
    },
    passwordComparison() {
      return this.LoginPassword;
    },
    handleSubmit() {
      let validationResult = this.validationGroup.validate();
      const buttonInstance = this.$refs["submitButton"].instance;
      if (
        !this.showToEdit &&
        validationResult.isValid &&
        this.isRecaptchaVerified
      ) {
        validationResult.status === "pending" &&
          validationResult.complete.then((res) => {
            if (res.isValid) {
              buttonInstance.option("icon", "fas fa-circle-notch fa-spin");
              buttonInstance.option("text", "Dodaję zawodnika...");
              this.addPlayer().then(() => {
                buttonInstance.option("icon", "");
                buttonInstance.option("text", this.getSubmitText());
                useToast().success("Zawodnik dodany pomyślnie!");
                this.$router.push({ path: "/" });
              });
            }
          });
      } else if (this.showToEdit && validationResult.isValid) {
        buttonInstance.option("icon", "fas fa-circle-notch fa-spin");
        buttonInstance.option("text", "Edytuję profil");
        this.editPlayer().then(() => {
          buttonInstance.option("icon", "");
          buttonInstance.option("text", this.getSubmitText());
          useToast().success("Edycja zakończona pomyślnie!");
          this.$router.push({ path: "/profile" });
        });
      }
    },
    recaptchaVerified() {
      this.isRecaptchaVerified = true;
    },
    recaptchaExpired() {
      this.isRecaptchaVerified = false;
      this.$refs.vueRecaptcha.reset();
    },
    recaptchaFailed() {
      this.isRecaptchaVerified = false;
    },
    imageAdded(e) {
      var reader = new FileReader();
      var fileByteArray = [];
      reader.readAsArrayBuffer(e.value[0]);
      reader.onloadend = function(evt) {
        if (evt.target.readyState == FileReader.DONE) {
          var arrayBuffer = evt.target.result,
            array = new Uint8Array(arrayBuffer);
          for (var i = 0; i < array.length; i++) {
            fileByteArray.push(array[i]);
          }
        }
      };
      this.Image = fileByteArray;
    },
    getSubmitText() {
      if (this.showToEdit) {
        return "Edytuj profil";
      } else {
        return "Zarejestruj zawodnika";
      }
    },
  },
  mounted() {
    if (this.$route.params.playerToEditId) {
      this.showToEdit = true;
    }
    this.RESET_PLAYER_FORM();
    if (this.showToEdit) {
      if (this.getLoggedInUser) {
        switch (this.getLoggedInUser.userType) {
          case userTypeEnum.PLAYER:
            this.setPlayerDetails(this.$route.params.playerToEditId);
            break;
          default:
            this.setUserDataDetails(this.$route.params.playerToEditId);
            break;
        }
      }
    }
    this.setAllTeams();
  },
  components: {
    DxTextBox,
    DxSelectBox,
    DxButton,
    DxValidator,
    DxRequiredRule,
    DxEmailRule,
    DxCompareRule,
    DxValidationGroup,
    DxValidationSummary,
    DxAsyncRule,
    vueRecaptcha,
    DxDateBox,
    DxFileUploader,
  },
  beforeUnmount() {
    this.RESET_PLAYER_FORM();
  },
};
</script>
