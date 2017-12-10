<template>
  <div class="wine-details">
    <div class="left-column">
      <h3>Details</h3>
      <div> {{wine.Name}}</div>
      <div>{{wine.Producer}}</div>
      <div>{{wine.Price}}</div>
    </div>
    <div class="right-column">
      <img v-bind:src="imageUrl" alt="wine image">
    </div>
  </div>
</template>

<script>
import wineService from "../services/wines";

export default {
  name: "WineListing",
  props: ["id"],
  data() {
    return {
      wine: {},
      imageUrl: "",
      error: ""
    };
  },
  created() {
    wineService
      .get(this.id)
      .then(response => {
        this.wine = response.data;
        this.imageUrl = `https://bilder.vinmonopolet.no/cache/515x515-0/${this
          .wine.VinmonopoletId}-1.jpg`;
      })
      .catch(e => {
        this.error = e;
      });
  }
};
</script>

<style scoped>
.wine-details {
  border: solid;
  padding-left: 10px;
  padding-bottom: 10px;
}

.left-column {
  float: left;
  width: 80%;
}

.right-column {
  clear: right;
}
</style>
