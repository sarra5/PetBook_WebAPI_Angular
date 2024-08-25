/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
    "./Pages/**/*.{html,ts}",
    "./Core/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      fontFamily:{
        aclonica: ['Aclonica'],
        glutin:['Gluten'],
        inter:['Inter'],
        Laila:['Laila'],
        Bitter:['Bitter'],
        Laila2:['Nunito Sans']

      }
    },
  },
  plugins: [],
}

