/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        copy: {
            main: {
                expand: true,
                cwd: 'Web/App_Plugins/',
                src: 'uSlack/**',
                dest: '../uSlack.Web/App_Plugins/'
            }
        },
        watch: {
            uSyncWeb: {
                files: ['Web/App_Plugins/uSlack/**/*.*'],
                tasks: ['copy']
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-watch');
};