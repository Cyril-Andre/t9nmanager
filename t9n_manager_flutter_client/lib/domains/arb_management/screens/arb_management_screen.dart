import 'dart:io';

import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/domains/arb_management/apis/api_arb_management.dart';

import 'package:t9n_manager_flutter_client/generated/l10n.dart';
import 'package:t9n_manager_flutter_client/shared/alert.dart';
import 'package:t9n_manager_flutter_client/shared/app_settings.dart';
import 'package:t9n_manager_flutter_client/shared/app_state_notifier.dart';
import 'package:t9n_manager_flutter_client/domains/project/models/project.dart';

class ArbManagementScreen extends StatefulWidget {
  const ArbManagementScreen({Key? key}) : super(key: key);

  @override
  State<ArbManagementScreen> createState() => _ArbManagementScreenState();
}

class _ArbManagementScreenState extends State<ArbManagementScreen> {
  bool toBeRefreshed = false;
  Project currentProject = Project("_", "(none)");

  @override
  void initState() {
    toBeRefreshed = false;
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    AppSettings appSettings = context.watch<AppSettings>();
    AppState appState = context.watch<AppState>();

    var jwt = appState.jwt;

    var currentTenant = appState.selectedTenant;
    var currentProject = appState.selectedProject;

    double width = MediaQuery.of(context).size.width > context.watch<AppState>().screenMaxSize ? context.watch<AppState>().screenMaxSize : MediaQuery.of(context).size.width;

    return Scaffold(
      appBar: AppBar(
        title: Text(S.of(context).title),
        leading: IconButton(
          icon: const Icon(Icons.abc),
          onPressed: close,
        ),
        actions: [
          IconButton(onPressed: () => {}, tooltip: "Download Arb file", icon: Icon(Icons.download)),
          IconButton(
              onPressed: () async {
                FilePickerResult? result = await FilePicker.platform.pickFiles(dialogTitle: "Select ARB file", type: FileType.custom, allowedExtensions: ["arb"]);
                if (result != null) {
                  var fileContent = result.files.single.bytes?.toList();
                  if (fileContent!=null) {
                    var response = postUploadArbFile(appSettings, jwt, context, fileContent);
                  }
                }
              },
              tooltip: "Upload Arb file",
              icon: Icon(Icons.upload)),
          Icon(Icons.add),
          Icon(Icons.delete)
        ],
      ),
    );
  }

  close() {
    Navigator.pop(context);
  }
}
