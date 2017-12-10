<template>
  <router-link class="wine-link" v-bind:to="wine.Id">
    <div class="wine-listing">
      <div class="left-column">
        <div class="wine-prop"> {{wine.Name}}</div>
        <div class="wine-prop">{{wine.Producer}}</div>
        <div class="wine-prop">{{wine.Price}}</div>
      </div>
      <div class="right-column">
        <div>
          <img v-bind:src="wineIconPath" height="50" width="50">
        </div>
        <div>
          <img v-bind:src="flagPath" height="50" width="50">
        </div>
      </div>
    </div>
  </router-link>
</template>

<script>
export default {
  name: "WineListing",
  props: ["wine"],
  data() {
    return {
      wineIconPath: `/images/${this.wine.Type}.svg`,
      flagPath: `/images/${this.wine.Country}.svg`
    };
  }
};
</script>

<style scoped>
.wine-listing {
  width: 300px;
  height: 100px;
  padding: 5px;
  border: solid;
}

.left-column {
  float: left;
  width: 80%;
}

.right-column {
  clear: right;
}

.wine-prop {
  white-space: nowrap;
  padding: 2px;
  overflow: hidden;
  text-overflow: ellipsis;
}
a:link,
a:visited {
  color: black;
}
a:hover {
  color: darkblue;
}
</style>
