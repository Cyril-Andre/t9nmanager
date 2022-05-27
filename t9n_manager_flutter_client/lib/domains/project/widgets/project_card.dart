import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/domains/project/apis/api_project.dart';
import 'package:t9n_manager_flutter_client/domains/tenant/apis/api_tenant.dart';
import 'package:t9n_manager_flutter_client/domains/tenant/models/invitation.dart';
import 'package:t9n_manager_flutter_client/domains/tenant/models/tenant.dart';
import 'package:t9n_manager_flutter_client/shared/token_helper.dart';
import 'package:t9n_manager_flutter_client/shared/widgets/confirm_dialog.dart';
import 'package:t9n_manager_flutter_client/shared/widgets/input_dialog.dart';
import 'package:t9n_manager_flutter_client/shared/widgets/spacer.dart';

import '../../../generated/l10n.dart';
import '../../../shared/app_settings.dart';
import '../../../shared/app_state_notifier.dart';
import '../models/project.dart';

class ProjectCard extends StatefulWidget {
  const ProjectCard({Key? key, required this.project, required this.addNew, required this.refresh}) : super(key: key);
  final Project project;
  final bool addNew;
  final VoidCallback refresh;

  @override
  State<ProjectCard> createState() => _ProjectCardState();
}

class _ProjectCardState extends State<ProjectCard> {
  InputDialogController inputDialogController = InputDialogController();
  ConfirmDialogController confirmDialogController = ConfirmDialogController();

  @override
  Widget build(BuildContext context) {
    AppSettings appSettings = context.watch<AppSettings>();
    AppState appState = context.watch<AppState>();
    var jwt = appState.jwt;
    var currentUser = TokenHelper.parseJwt(jwt)['name'];
    var currentTenant = appState.selectedTenant;
    Icon icon = const Icon(Icons.abc);
    if (widget.addNew) {
      icon = const Icon(Icons.bookmark_add_outlined);
    } else {
      icon = const Icon(Icons.post_add_rounded);
    }
    double cardSize = context.watch<AppState>().cardSize;
    return SizedBox(
      width: cardSize,
      height: cardSize,
      child: GestureDetector(
        onTap: () {
          if (widget.project.id != "_") {
            changeProject(widget.project, context);
          }
        },
        child: Card(
          elevation: 8.0,
          shadowColor: Colors.black87,
          margin: const EdgeInsets.all(8),
          child: Column(
            mainAxisSize: MainAxisSize.max,
            children: [
              const Icon(
                Icons.abc,
                size: 30.0,
              ),
              verticalSpaceRegular,
              Text(
                widget.project.name,
                textAlign: TextAlign.center,
                style: const TextStyle(fontSize: 14),
              ),
              ButtonBar(
                alignment: MainAxisAlignment.end,
                mainAxisSize: MainAxisSize.min,
                buttonMinWidth: 4.0,
                buttonHeight: 4.0,
                buttonPadding: const EdgeInsets.all(1.0),
                children: [
                  if (widget.addNew)
                    InputDialog(
                        controller: inputDialogController,
                        callToActionIcon: icon,
                        title: S.of(context).project_card_add_project_title,
                        message: S.of(context).project_card_add_project_message,
                        toolTip: S.of(context).project_card_add_project_tooltip,
                        callBack: () async => await onAddProject(currentTenant, appSettings, jwt, context)),
                  if (!widget.addNew)
                    IconButton(
                        tooltip: S.of(context).tenant_card_select_tenant_tooltip,
                        onPressed: () {
                          changeProject(widget.project, context);
                        },
                        icon: const Icon(Icons.check_box)),
                ],
              )
            ],
          ),
        ),
      ),
    );
  }

  changeProject(Project project, BuildContext context) {
    context.read<AppState>().setProject(project);
    Navigator.pop(context);
  }

  onAddProject(Tenant selectedTenant, AppSettings appSettings, String jwt, BuildContext context) async {
    String projectName = inputDialogController.value;
    await createProject(appSettings, jwt, context, selectedTenant, projectName);
    widget.refresh();
  }
}
