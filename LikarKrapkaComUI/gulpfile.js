const gulp = require('gulp');
const sass = require('gulp-sass');
const autoprefixer = require('gulp-autoprefixer');
const cleanCss = require('gulp-clean-css');
const concat = require('gulp-concat');
const rename = require('gulp-rename');
const merge = require('merge-stream');

const dir = {
    src: './wwwroot/scss/',
    build: './wwwroot/css/'
}

const css = {
    name: 'style',
    src: dir.src + 'main.scss',
    watch: dir.src + '**/*',
    build: dir.build + '',
};


function stylesProcessor() {
    const streams = [];

    const pipe = gulp.src(css.src)
        .pipe(sass().on('error', sass.logError))
        .pipe(concat(`${css.name}.css`))
        .pipe(autoprefixer({
            cascade: false
        }))
        .pipe(cleanCss({ level: 2 }))
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(css.build));

    streams.push(pipe);


    return merge(streams);
}

function watch() {
    const listWatch = [];
    listWatch.push(css.watch);
    gulp.watch(listWatch, { usePolling: true }, stylesProcessor);
}

gulp.task("default", gulp.series(stylesProcessor, watch));
gulp.task("build", gulp.series(stylesProcessor));
