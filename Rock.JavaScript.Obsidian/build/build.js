const path = require("path");
const webpack = require("webpack");
const fs = require("fs");
const glob = require("glob");

async function getTypeScriptFiles() {
    const files = await new Promise((resolve, reject) => {
        glob("./Framework/**/*.ts", {
            ignore: [
                "./Framework/Tests/**",
                "./Framework/**/*.d.ts"
            ]
        }, (err, matches) => {
            if (err) {
                return reject(err);
            }

            resolve(matches);
        });
    });

    return files;
}

function externalsHandler({ context, request, contextInfo, getResolve }, callback) {
    if (contextInfo.issuer === "") {
        return callback();
    }
    callback(null, "system " + request);
}

const baseConfig = {
    mode: "development",
    devtool: process.env.CONFIGURATION === "Debug" ? "source-map" : false,
    output: {
        path: path.resolve(__dirname, "../dist2"),
        filename: "[name].js",
    },
    externals: externalsHandler,
    resolve: {
        extensions: [".ts", ".js"]
    },
    /* Enable caching so rebuilds are faster. */
    cache: {
        type: 'filesystem',
        buildDependencies: {
            config: [__filename],
        },
    },
    module: {
        rules: [
            /* all files with a `.ts` extension will be handled by `ts-loader`. */
            {
                test: /\.ts$/,
                loader: "ts-loader"
            },
        ],
    },
    parallelism: 4,
    plugins: [
        new webpack.NoEmitOnErrorsPlugin()
    ],
    /* Warn if any file goes over 100KB. */
    performance: {
        maxEntrypointSize: 100000,
        maxAssetSize: 100000
    }
};

async function isOutputUpToDate(input) {
    let outputFilename = path.resolve(__dirname, "..", "dist2", input);
    outputFilename = outputFilename.substr(0, outputFilename.length - 3) + ".js";

    const inputStat = await fs.promises.stat(input);

    try {
        const outputStat = await fs.promises.stat(outputFilename);

        return outputStat.mtimeMs > inputStat.mtimeMs;
    } catch {
        return false;
    }
}

async function build() {
    /** @type string[] */
    const files = await getTypeScriptFiles();
    const entry = {};

    for (const file of files) {
        if (await isOutputUpToDate(file)) {
            continue;
        }

        console.log(file);

        entry[file] = {
            import: file,
            filename: file.substr(0, file.length - 3) + ".js",
            library: {
                type: "system"
            }
        };
    }

    const compiler = webpack({
        entry: entry,
        ...baseConfig
    });

    // `compiler.run()` doesn't support promises yet, only callbacks
    //await new Promise((resolve, reject) => {
//        compiler.run((err, stats) => {
    compiler.watch({}, (err, stats) => {
            //if (err) {
            //    return reject(err);
            //}

            console.log(stats.toString({
                modules: false,
                errorDetails: true,
                timings: true,
                cachedModules: true,
                colors: true,
                runtimeModules: true
            }));

            //resolve(stats);
        });
//    });
}

build();
