<template>
    <div id="wine-list">
        <wine-listing v-for="wine in wines" :wine="wine" :key="wine.Name" />
    </div>
</template>

<script>
import WineListing from './WineListing.vue'

export default {
    name: 'WineList',
    props: ['wines'],
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
