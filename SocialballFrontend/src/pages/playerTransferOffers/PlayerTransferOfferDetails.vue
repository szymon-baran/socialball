<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      :width="400"
      :height="520"
      container=".dx-viewport"
      :title="getTitle()"
      :shading="false"
      v-if="playerTransferOffer.Id"
    >
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="before"
        :options="closeButtonOptions"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="rejectButtonOptions"
        v-if="!isReadOnly"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="acceptButtonOptions"
        v-if="!isReadOnly"
      />
      <h4>
        {{ playerTransferOffer.FromTeamName }} składa ofertę za
        {{ playerTransferOffer.PlayerName }}!
      </h4>
      <div class="mt-4">
        <p>Kwota transferu: {{ playerTransferOffer.TransferFee }} PLN</p>
        <p>
          Przyszłe zarobki zawodnika:
          {{ playerTransferOffer.PlayerEarnings }} PLN/miesiąc
        </p>
        <p>{{ playerTransferOffer.Content }}</p>
      </div>
      <div class="mt-4">
        <h4>Stan transferu:</h4>
        <ul>
          <li>
            <span v-if="!playerTransferOffer.IsAcceptedByOtherTeam"
              >Oferta wymaga akceptacji przez</span
            >
            <span v-else>Oferta została już zaakceptowana przez</span>
            aktualną drużynę zawodnika ({{ playerTransferOffer.ToTeamName }})
          </li>
          <li>
            <span v-if="!playerTransferOffer.IsAcceptedByPlayer"
              >Oferta wymaga akceptacji przez</span
            >
            <span v-else>Oferta została już zaakceptowana przez</span>
            zawodnika
          </li>
        </ul>
      </div>
    </DxPopup>
  </div>
</template>
<script>
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import { mapActions, mapGetters, mapMutations, mapState } from "vuex";
import { custom } from "devextreme/ui/dialog";

export default {
  name: "PlayerTransferOfferDetails",
  props: {
    showAsTeam: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      popupVisible: true,
      closeButtonOptions: {
        text: "Zamknij",
        onClick: () => {
          this.popupVisible = false;
          this.$emit("closed");
        },
        type: "normal",
      },
      rejectButtonOptions: {
        text: "Odrzuć",
        onClick: () => {
          this.reject();
        },
        type: "danger",
      },
      acceptButtonOptions: {
        text: "Zaakceptuj",
        onClick: () => {
          this.accept();
        },
        type: "default",
      },
    };
  },
  components: {
    DxPopup,
    DxToolbarItem,
  },
  computed: {
    ...mapState("playerTransferOffers", ["playerTransferOffer"]),
    ...mapGetters({ getLoggedInUser: "authentication/getLoggedInUser" }),
    isReadOnly() {
      if (this.showAsTeam) {
        if (this.playerTransferOffer.IsAcceptedByOtherTeam) {
          return true;
        } else {
          return false;
        }
      } else {
        if (this.playerTransferOffer.IsAcceptedByPlayer) {
          return true;
        } else {
          return false;
        }
      }
    },
  },
  methods: {
    ...mapActions({
      rejectOffer: "playerTransferOffers/rejectOffer",
      acceptOfferAsTeam: "playerTransferOffers/acceptOfferAsTeam",
      acceptOfferAsPlayer: "playerTransferOffers/acceptOfferAsPlayer",
      setPlayerTransferOfferDetails:
        "playerTransferOffers/setPlayerTransferOfferDetails",
    }),
    ...mapMutations({
      RESET_PLAYER_TRANSFER_OFFER_FORM:
        "playerTransferOffers/RESET_PLAYER_TRANSFER_OFFER_FORM",
    }),
    getTitle() {
      return (
        this.playerTransferOffer.PlayerName +
        " -> " +
        this.playerTransferOffer.FromTeamName
      );
    },
    reject() {
      let dialog = custom({
        title: "Odrzucenie",
        messageHtml:
          "Czy na pewno chcesz odrzucić ofertę " +
          this.playerTransferOffer.PlayerName +
          " -> " +
          this.playerTransferOffer.FromTeamName +
          "?",
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
          this.rejectOffer(this.playerTransferOffer);
          this.popupVisible = false;
          this.$emit("closed");
        }
      });
    },
    accept() {
      let warning = "";
      if (
        (this.showAsTeam && this.playerTransferOffer.IsAcceptedByPlayer) ||
        (!this.showAsTeam && this.playerTransferOffer.IsAcceptedByOtherTeam)
      ) {
        warning =
          "<br>Zaakceptuj ofertę dopiero po uregulowaniu płatności finansowych!<br>Po Twojej akceptacji, transfer zostanie dokonany!";
      }
      let dialog = custom({
        title: "Akceptacja",
        messageHtml:
          "Czy na pewno chcesz zaakceptować ofertę " +
          this.playerTransferOffer.PlayerName +
          " -> " +
          this.playerTransferOffer.FromTeamName +
          "?" +
          warning,
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
          if (this.showAsTeam) {
            this.acceptOfferAsTeam();
          } else {
            this.acceptOfferAsPlayer(this.getLoggedInUser.id);
          }
          this.popupVisible = false;
          this.$emit("closed");
        }
      });
    },
  },
  mounted() {
    this.popupVisible = true;
  },
  beforeUnmount() {
    this.RESET_PLAYER_TRANSFER_OFFER_FORM();
  },
};
</script>
