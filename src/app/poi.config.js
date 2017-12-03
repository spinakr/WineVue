module.exports = (options, req) => ({
  entry: "./src/index.js",
  devServer: {
    proxy: "http://localhost:8080/api"
  }
});
