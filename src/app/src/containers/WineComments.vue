<template>
  <div class="wine-comments">
  </div>
</template>

<script>
import wineService from "../services/wines";

export default {
  name: "WineComments",
  props: ["vinmonopoletId"],
  data() {
    return {
      comments: {},
      error: ""
    };
  },
  created() {
    wineService
      .get(this.vinmonopoletId)
      .then(response => {
        this.comments = response.data;
      })
      .catch(e => {
        this.error = e;
      });
  }
};
</script>

<style scoped>
.wine-comments {
  border: solid;
  padding-left: 10px;
  padding-bottom: 10px;
}
</style>
