<template>
  <div id="app">
    <img alt="Vue logo" src="./assets/logo.png">
    <CourseList />
  </div>
</template>

<script>
import CourseList from './components/CourseList.vue'

export default {
  name: 'App',
  components: {
    CourseList
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
  background-color: #000000;
}
</style>
