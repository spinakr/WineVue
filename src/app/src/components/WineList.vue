<template>
    <div id="wine-list">
        <div v-if="error">
            {{error}}
        </div>
        <wine-listing v-for="wine in wines" :wine="wine" :key="wine.Name" />
    </div>
</template>

<script>
import WineListing from './WineListing.vue'
import wineService from '../services/wines';

export default {
    name: 'WineList',
    data () {
        return {
            wines: [],
            error: ''
        }
    },
    created () {
        wineService.get(this.$route.name)
            .then(response => {
                this.wines = response.data.Wines
            })
            .catch(e => {
                this.error = e
            });
    },
    components: {
        WineListing 
    }
}
</script>

<style scooped>

#wine-list {
    display: grid;
    grid-gap: 10px;
    justify-content: center;
}

@media (min-width: 620px) {
  #wine-list {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (min-width: 1000px) {
  #wine-list {
    grid-template-columns: repeat(3, 1fr);
  }
}
</style>
