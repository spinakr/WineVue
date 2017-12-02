<template>
  <div class="wine-details">
    <h3>Details</h3>
    <div> {{wine.Name}}</div>
    <div>{{wine.Producer}}</div>
    <div>{{wine.Price}}</div>
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
      error: ""
    };
  },
  created() {
    wineService
      .get(this.id)
      .then(response => {
        this.wine = response.data;
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
</style>
