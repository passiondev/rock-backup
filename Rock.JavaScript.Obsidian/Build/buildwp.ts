import * as path from "path";
import * as webpack from "webpack";
import * as fs from "fs";
import * as glob from "glob";

async function getTypeScriptFiles(): Promise<string[]> {
    const files = await new Promise((resolve, reject) => {
        glob("./Framework/Rules/*.ts", {
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

    return files as string[];
}

/**
 * An object with entry point description.
 */
declare interface IEntryDescription {
    /**
     * The method of loading chunks (methods included by default are 'jsonp' (web), 'import' (ESM), 'importScripts' (WebWorker), 'require' (sync node.js), 'async-node' (async node.js), but others might be added by plugins).
     */
    chunkLoading?: string | false;

    /**
     * The entrypoints that the current entrypoint depend on. They must be loaded when this entrypoint is loaded.
     */
    dependOn?: string | string[];

    /**
     * Specifies the filename of the output file on disk. You must **not** specify an absolute path here, but the path may contain folders separated by '/'! The specified path is joined with the value of the 'output.path' option to determine the location on disk.
     */
    filename?: string;

    /**
     * Module(s) that are loaded upon startup.
     */
    import: string;

    /**
     * Specifies the layer in which modules of this entrypoint are placed.
     */
    layer?: null | string;

    /**
     * Options for library.
     */
    //library?: LibraryOptions;

    /**
     * The 'publicPath' specifies the public URL address of the output files when referenced in a browser.
     */
    publicPath?: string;

    /**
     * The name of the runtime chunk. If set a runtime chunk with this name is created or an existing entrypoint is used as runtime.
     */
    runtime?: string | false;

    /**
     * The method of loading WebAssembly Modules (methods included by default are 'fetch' (web/WebWorker), 'async-node' (node.js), but others might be added by plugins).
     */
    wasmLoading?: string | false;
}


declare interface IModuleFactoryCreateDataContextInfo {
    issuer: string;
    issuerLayer?: null | string;
    compiler: string;
}

/**
 * Data object passed as argument when a function is set for 'externals'.
 */
declare interface IExternalItemFunctionData {
    /**
     * The directory in which the request is placed.
     */
    context?: string;

    /**
     * Contextual information.
     */
    contextInfo?: IModuleFactoryCreateDataContextInfo;

    /**
     * The category of the referencing dependencies.
     */
    dependencyType?: string;

    /**
     * Get a resolve function with the current resolver options.
     */
    getResolve?: (
        options?: unknown
    ) =>
        | ((
            context: string,
            request: string,
            callback: (err?: Error, result?: string) => void
        ) => void)
        | ((context: string, request: string) => Promise<string>);

    /**
     * The request as written by the user in the require/import expression/statement.
     */
    request?: string;
}
function externalsHandler(external: IExternalItemFunctionData, callback: (
    err?: Error,
    result?: string | boolean | string[] | { [index: string]: any }
) => void): void {
    if (external.contextInfo.issuer === "") {
        return callback();
    }

    callback(null, "system " + external.request);
}

const baseConfig: webpack.Configuration = {
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
        type: "filesystem",
        buildDependencies: {
            config: [__filename],
        },
    },
    module: {
        rules: [
            /* all files with a `.ts` extension will be handled by `ts-loader`. */
            {
                test: /\.ts$/,
                loader: "ts-loader",
                options: {
                    compilerOptions: {
                        outDir: "../dist2/Framework",
                        composite: false,
                        declaration: false
                    }
                }
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

async function isOutputUpToDate(input: string): Promise<boolean> {
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

async function build(): Promise<void> {
    /** @type string[] */
    const files = await getTypeScriptFiles();
    const entry: Record<string, any> = {};

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
    //    compiler.run((err, stats) => {
    //        if (err) {
    //            return reject(err);
    //        }

    //        console.log(stats.toString({
    //            modules: false,
    //            errorDetails: true,
    //            timings: true,
    //            cachedModules: true,
    //            colors: true,
    //            runtimeModules: true
    //        }));

    //        resolve(stats);
    //    });
    //});

    compiler.watch({}, (err, stats) => {
        console.log(stats.toString({
            modules: false,
            errorDetails: true,
            timings: true,
            cachedModules: true,
            colors: true,
            runtimeModules: true
        }));
    });
}

build();
